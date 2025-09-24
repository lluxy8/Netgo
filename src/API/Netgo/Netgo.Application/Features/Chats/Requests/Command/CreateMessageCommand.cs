using MediatR;
using Netgo.Application.Common;
using Netgo.Application.DTOs.Message;

namespace Netgo.Application.Features.Chat.Requests.Command
{
    public class CreateMessageCommand : IRequest<Result<MessageSentDTO>>
    {
        public required CreateMessageDTO MessageDTO { get; set; }
    }
}
