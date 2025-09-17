using MediatR;
using Netgo.Application.Common;
using Netgo.Application.DTOs.Message;

namespace Netgo.Application.Features.Chat.Requests.Command
{
    public class UpdateMessageCommand : IRequest<Result<MessageSentDTO>>
    {
        public required UpdateMessageDTO MessageDTO { get; set; }
    }
}
