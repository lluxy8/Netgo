using AutoMapper;
using MediatR;
using Netgo.Application.Common;
using Netgo.Application.Contracts.Persistence;
using Netgo.Application.DTOs.Message;
using Netgo.Application.DTOs.Message.Validators;
using Netgo.Application.Exceptions;
using Netgo.Application.Features.Chat.Requests.Command;
using Netgo.Domain;

namespace Netgo.Application.Features.Chat.Handlers.Command
{
    public class UpdateMessageCommandHandler : IRequestHandler<UpdateMessageCommand, Result<MessageSentDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateMessageCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<MessageSentDTO>> Handle(UpdateMessageCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateMessageDTOValidator();
            var validationResult = await validator.ValidateAsync(request.MessageDTO);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult);

            var message = await _unitOfWork.Messages.GetById(request.MessageDTO.Id) 
                ?? throw new NotFoundException(nameof(Message), request.MessageDTO.Id);

            message.OldContents = message.OldContents ?? [];
            message.OldContents.Add(message.Content);
            message.Content = request.MessageDTO.Content;
            message.DisplayContent = request.MessageDTO.Content;
            message.DateModified = DateTime.UtcNow;

            await _unitOfWork.Messages.Update(message);

            return Result<MessageSentDTO>.Success(new MessageSentDTO
            {
                MessageId = message.Id,
                ChatId = message.ChatId
            });
        }
    }
}
