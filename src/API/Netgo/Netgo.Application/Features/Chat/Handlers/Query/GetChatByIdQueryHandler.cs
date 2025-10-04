using AutoMapper;
using MediatR;
using Netgo.Application.Common;
using Netgo.Application.Contracts.Persistence;
using Netgo.Application.DTOs.Chat;
using Netgo.Application.DTOs.Message;
using Netgo.Application.Exceptions;
using Netgo.Application.Features.Chat.Requests.Query;
using Netgo.Domain;

namespace Netgo.Application.Features.Chat.Handlers.Query
{
    public class GetChatByIdQueryHandler : IRequestHandler<GetChatByIdQuery, Result<ChatDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetChatByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<ChatDTO>> Handle(GetChatByIdQuery request, CancellationToken cancellationToken)
        {
            var chat = await _unitOfWork.Chats.GetChatWithMessages(request.Id)
                ?? throw new NotFoundException(nameof(Chat), request.Id);

            var messages = _mapper.Map<List<MessageDTO>>(chat.Messages);
            var chatDTO = _mapper.Map<ChatDTO>(chat);
            chatDTO.Messages = messages;

            return Result<ChatDTO>.Success(chatDTO);
        }
    }
}
