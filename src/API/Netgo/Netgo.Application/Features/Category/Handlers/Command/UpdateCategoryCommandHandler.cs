using AutoMapper;
using MediatR;
using Netgo.Application.Common;
using Netgo.Application.Contracts.Persistence;
using Netgo.Application.DTOs.Category.Validators;
using Netgo.Application.Exceptions;
using Netgo.Application.Features.Category.Requests.Command;
using Netgo.Domain;

namespace Netgo.Application.Features.Category.Handlers.Command
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;   
        }



        public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var valdiator = new CategoryUpdateDTOValidatior();
            var validationResult = await valdiator.ValidateAsync(request.CategoryDTO, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult);

            var existingCategory = await _unitOfWork.Categories.GetById(request.CategoryDTO.Id)
                ?? throw new NotFoundException(nameof(Product), request.CategoryDTO.Name);

            var category = _mapper.Map(request.CategoryDTO, existingCategory);

            await _unitOfWork.Categories.Update(category);

            return Result.Success();
        }
    }
}
