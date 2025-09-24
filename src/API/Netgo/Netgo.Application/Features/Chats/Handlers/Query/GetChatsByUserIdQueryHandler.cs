using AutoMapper;
using MediatR;
using Netgo.Application.Common;
using Netgo.Application.Contracts.Persistence;
using Netgo.Application.DTOs.Chat;
using Netgo.Application.Features.Chat.Requests.Query;

namespace Netgo.Application.Features.Chat.Handlers.Query
{
    internal class GetChatsByUserIdQueryHandler : IRequestHandler<GetChatsByUserIdQuery, Result<List<ListChatDTO>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetChatsByUserIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<ListChatDTO>>> Handle(GetChatsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var chats = await _unitOfWork.Chats.GetChatsByUserId(request.Id);
            var chatsDTO = _mapper.Map<List<ListChatDTO>>(chats);
            return Result<List<ListChatDTO>>.Success(chatsDTO);
        }
    }
}
