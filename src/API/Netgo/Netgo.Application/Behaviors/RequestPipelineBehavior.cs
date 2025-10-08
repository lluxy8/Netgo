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

            var requestName = typeof(TRequest).Name;
            try
            {
                var sw = Stopwatch.StartNew();

                TResponse response;

                if (requestName.Contains("Query"))
                {
                    response = await next(cancellationToken);
                }
                else if (requestName.Contains("Command"))
                {
                    await _unitOfWork.BeginTransactionAsync(cancellationToken);

                    response = await next(cancellationToken);

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
                    "Request '{Request}' executed in {Duration:F2} ms",
                    requestName,
                    duration.TotalMilliseconds
                );

                return response;

            }
            catch (Exception ex)
            {
                var statusCode = ex switch
                {
                    NotFoundException => 404,
                    ValidationException => 400,
                    BadRequestException => 400,
                    UnauthorizedException => 401,
                    UnauthorizedAccessException => 403,
                    _ => 500
                };

                if (statusCode == 500)
                {
                    _logger.LogError(
                        ex,
                        "An unhandled exception occurred while processing {RequestType}.",
                        typeof(TRequest).Name);

                    return CreateFailureResponse(statusCode, "Internal server error");
                }

                _logger.LogWarning("" +
                    "Request '{Request}' failed with StatusCode: {StatusCode}.", 
                    requestName, 
                    statusCode);

                var response = CreateFailureResponse(statusCode, ex.Message);
                return response;
            }
        }


        private TResponse CreateFailureResponse(int status, string message)
        {
            if (typeof(TResponse).IsGenericType &&
                typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
            {
                var resultType = typeof(TResponse);
                var method = resultType.GetMethod("Failure", new[] { typeof(string), typeof(int) })
                    ?? throw new InvalidOperationException("Failure method not found");

                return (TResponse)method.Invoke(null, [message, status])!;
            }


            return (TResponse)(object)Result.Failure(message, status);
        }
    }
}
