using MediatR;
using Netgo.Application.Common;
using Netgo.Application.DTOs.Chat;

namespace Netgo.Application.Features.Chat.Requests.Query
{
    public class GetChatsByUserIdQuery : IRequest<Result<List<ListChatDTO>>>
    {
        public Guid Id { get; set; }
    }
}
