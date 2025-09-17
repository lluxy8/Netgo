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
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Result>
    {
        private readonly IMapper _mapper;

        public CreateCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        private readonly IUnitOfWork _unitOfWork;

        public async Task<Result> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var validator = new CategoryCreateDTOValidator();
            var validationResult = await validator.ValidateAsync(request.CategoryDto);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult);

           var categoryWithNameExists = 
                await _unitOfWork.Categories.CategoryExistsByName(request.CategoryDto.Name);

            if (categoryWithNameExists)
                throw new BadRequestException($"Category with that name {request.CategoryDto.Name} already exists");

            var category = _mapper.Map<Domain.Category>(request.CategoryDto);

            await _unitOfWork.Categories.Insert(category);

            return Result.Success();
        }
    }
}
