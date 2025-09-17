using MediatR;
using Netgo.Application.Common;
using Netgo.Application.DTOs.Chat;

namespace Netgo.Application.Features.Chat.Requests.Query
{
    public class GetChatByIdQuery : IRequest<Result<ChatDTO>>
    {
        public Guid Id { get; set; }
    }
}
