using MediatR;
using Microsoft.Extensions.Logging;
using Netgo.Application.Common;
using Netgo.Application.Contracts.Persistence;
using Netgo.Application.Exceptions;
using Netgo.Application.Features.Category.Requests.Command;

namespace Netgo.Application.Features.Category.Handlers.Command
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteCategoryCommandHandler> _logger;
        private static Guid defaultCategoryId = Guid.Parse("0D2EE86D-81B7-429F-B91F-BAB837D4BDCC");

        public DeleteCategoryCommandHandler(
            IUnitOfWork unitOfWork, 
            ILogger<DeleteCategoryCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == defaultCategoryId)
                throw new BadRequestException("Can't delete default category.");

            var category = await _unitOfWork.Categories.GetCategoryWithProducts(request.Id)
                ?? throw new NotFoundException("Category", request.Id);                

            var categoryToMove = await _unitOfWork.Categories.GetCategoryWithProducts(defaultCategoryId) 
                ?? throw new InvalidOperationException("Default category was not found.");

            try
            {
                _logger.LogInformation("Moving products to default category.");
                int i = 0;
                foreach (var product in category.Products)
                {
                    categoryToMove.Products.Add(product);
                    _logger.LogInformation("[{num}] ({id}) Moving...", i, product.Id);
                    i++;
                }

                _logger.LogInformation("Successfully moved {num} Product to default category.", i);
            }
            catch
            {
                _logger.LogCritical("Moving products failed.");
                throw;
            }

            await _unitOfWork.Categories.Delete(category);

            return Result.Success();
        }
    }
}
