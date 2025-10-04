using MediatR;
using Netgo.Application.Contracts.Persistence;
using Netgo.Application.Exceptions;
using Microsoft.Extensions.Logging;
using Netgo.Application.Common;
using System.Diagnostics;

namespace Netgo.Application.Behaviors
{
    public class RequestPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : Result
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RequestPipelineBehavior<TRequest, TResponse>> _logger;

        public RequestPipelineBehavior(
            IUnitOfWork unitOfWork, 
            ILogger<RequestPipelineBehavior<TRequest, TResponse>> logger)
        {
            _unitOfWork = unitOfWork; 
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                var sw = Stopwatch.StartNew();
                var requestName = typeof(TRequest).Name;

                TResponse response;

                if (requestName.Contains("Query"))
                {
                    response = await next();
                }
                else if (requestName.Contains("Command"))
                {
                    await _unitOfWork.BeginTransactionAsync(cancellationToken);

                    response = await next();

                    await _unitOfWork.CommitTransactionAsync(cancellationToken);
                }
                else
                {
                    throw new InvalidOperationException($"Unknown request type: {requestName}");
                }

                if (response is null)
                    throw new InvalidOperationException($"Handler for {requestName} returned null response.");

                var duration = sw.Elapsed;

                _logger.LogInformation(
                    "Request {RequestName} executed in {Duration:F2} ms",
                    requestName,
                    duration.TotalMilliseconds
                );

                return response;

            }
            catch (Exception ex)
            {
                switch(ex)
                {
                    case NotFoundException:
                        return CreateFailureResponse(ex.Message, 404);
                    case ValidationException:
                        return CreateFailureResponse(ex.Message, 400);
                    case BadRequestException:
                        return CreateFailureResponse(ex.Message, 400);
                    case UnauthorizedException:
                        return CreateFailureResponse(ex.Message, 401);

                    default:{
                        _logger.LogCritical(
                            ex,
                            "An unhandled exception occurred while processing {RequestType}.",
                            typeof(TRequest).Name);

                        return CreateFailureResponse("Inrernal Server Exception", 500);
                    }
                }
            }
        }

        private static TResponse CreateFailureResponse(string errorMessage, int statusCode)
        {
            var responseType = typeof(TResponse);
            var method = responseType.GetMethod("Failure", [typeof(string), typeof(int)]);
            var res = (TResponse)method.Invoke(null, [errorMessage, statusCode])
                ?? throw new InvalidOperationException();
            return res;
        }
    }
}
