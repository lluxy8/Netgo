using MediatR;
using Netgo.Application.Common;

namespace Netgo.Application.Features.Chats.Requests.Command
{
    public class DeleteMessageCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
    }
}
