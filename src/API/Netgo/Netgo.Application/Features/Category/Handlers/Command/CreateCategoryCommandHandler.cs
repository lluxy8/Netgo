using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateCategoryCommandHandler> _logger;

        public CreateCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateCategoryCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var validator = new CategoryCreateDTOValidator();
            var validationResult = await validator.ValidateAsync(request.CategoryDto, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult);

           var categoryWithNameExists = 
                await _unitOfWork.Categories.CategoryExistsByName(request.CategoryDto.Name);

            if (categoryWithNameExists)
                throw new BadRequestException($"Category with that name {request.CategoryDto.Name} already exists");

            var category = _mapper.Map<Domain.Category>(request.CategoryDto);

            await _unitOfWork.Categories.Insert(category);
            _logger.LogInformation("New category created {Name} ~ {Id}", category.Name, category.Id);

            return Result.Success();
        }
    }
}
