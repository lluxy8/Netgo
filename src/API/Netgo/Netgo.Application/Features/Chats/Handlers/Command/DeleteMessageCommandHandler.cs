using MediatR;
using Netgo.Application.Common;
using Netgo.Application.Contracts.Persistence;
using Netgo.Application.Exceptions;
using Netgo.Application.Features.Chats.Requests.Command;
using Netgo.Domain;

namespace Netgo.Application.Features.Chats.Handlers.Command
{
    public class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommand, Result>
    {
        private readonly IUnitOfWork _uniOfWork;

        public DeleteMessageCommandHandler(IUnitOfWork uniOfWork)
        {
            _uniOfWork = uniOfWork;
        }

        public async Task<Result> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
        {
            var message = await _uniOfWork.Messages.GetById(request.Id) 
                ?? throw new NotFoundException(nameof(Message), request.Id);

            message.DateDeleted = DateTime.UtcNow;

            return Result.Success();
        }
    }
}
