using AutoMapper;
using MediatR;
using Netgo.Application.Common;
using Netgo.Application.Contracts.Identity;
using Netgo.Application.Contracts.Persistence;
using Netgo.Application.DTOs.Message;
using Netgo.Application.DTOs.Message.Validators;
using Netgo.Application.Exceptions;
using Netgo.Application.Features.Chat.Requests.Command;
using Netgo.Domain;

namespace Netgo.Application.Features.Chat.Handlers.Command
{
    public class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommand, Result<MessageSentDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        public CreateMessageCommandHandler(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IUserService userService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        public async Task<Result<MessageSentDTO>> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateMessageDTOValidator();
            var validationResult = await validator.ValidateAsync(request.MessageDTO, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult);

            var usersExists =
                await _userService.UserExists(request.MessageDTO.From.ToString()) &&
                await _userService.UserExists(request.MessageDTO.To.ToString());

            if (!usersExists)
                throw new NotFoundException("Users", string.Empty);

            var chat = await _unitOfWork.Chats.GetChatByUsers(
                request.MessageDTO.From, request.MessageDTO.To);

            Domain.Chat? newChat = null;

            if (chat is null)
            {
                newChat = new Domain.Chat
                {
                    FirstUserId = request.MessageDTO.From,
                    SecondUserId = request.MessageDTO.To
                };

                await _unitOfWork.Chats.Insert(newChat);
            }

            var message = _mapper.Map<Message>(request.MessageDTO);
    
            message.DisplayContent = message.Content;
            message.Content = message.Content; // değişebilir
            message.UserId = request.MessageDTO.From;

            if (newChat != null)
                message.ChatId = newChat.Id;
            else if (chat != null)
                message.ChatId = chat.Id;

            await _unitOfWork.Messages.Insert(message);

            return Result<MessageSentDTO>.Success(new MessageSentDTO
            {
                ChatId = message.ChatId,
                MessageId = message.Id
            });
        }
    }
}
