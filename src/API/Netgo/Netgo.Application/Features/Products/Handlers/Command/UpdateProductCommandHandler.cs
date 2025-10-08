using AutoMapper;
using MediatR;
using Netgo.Application.Common;
using Netgo.Application.Contracts.Infrastructure;
using Netgo.Application.Contracts.Persistence;
using Netgo.Application.DTOs.Product;
using Netgo.Application.DTOs.Product.Validators;
using Netgo.Application.Exceptions;
using Netgo.Application.Features.Products.Requests.Command;
using Netgo.Domain;

namespace Netgo.Application.Features.Products.Handlers.Command
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IEmailService _emailService;

        public UpdateProductCommandHandler(
            IUnitOfWork unitOfWork, 
            IMapper mapper,
            IFileService fileService,
            IEmailService emailService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _fileService = fileService;
            _emailService = emailService;
        }
        public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Products.GetById(request.ProductDto.Id)
                ?? throw new NotFoundException(nameof(Product), request.ProductDto.Id);

            var authorized = product.UserId.ToString() == request.UserId;

            if (!authorized)
                throw new UnauthorizedAccessException("");

            var validator = new UpdateProductDTOValidator();
            var validationResult = await validator.ValidateAsync(request.ProductDto, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult);



            if (product.DateSold != null)
                throw new BadRequestException("Cannot update already sold product");

            var oldProduct = product;

            var categoryExists = await _unitOfWork.Categories.Exists(request.ProductDto.CategoryId);
            if(!categoryExists)
                throw new NotFoundException("Category", request.ProductDto.CategoryId);

            if (request.ProductDto.Images.Count != 0)
            {
                foreach (var image in request.ProductDto.Images)
                {
                    if (product.Images.Contains(image))
                        continue;

                    await _fileService.DeleteFileAsync(product.Id.ToString(), image);
                    product.Images.Remove(image);
                }
            }

            if (request.ProductDto.NewImages.Count != 0)
            {
                foreach (var image in request.ProductDto.NewImages)
                {
                    string path = await _fileService.SaveFileAsync(product.Id.ToString(), image);
                    product.Images.Add(path);
                }
            }

            var mappedProduct = _mapper.Map(request.ProductDto, product);
            mappedProduct.Details = _mapper.Map<List<ProductDetail>>(request.ProductDto.Details);
            foreach (var detail in mappedProduct.Details)
            {
                detail.ProductId = mappedProduct.Id;
            }

            await _unitOfWork.Products.Update(mappedProduct);
            product.Updated(oldProduct);

            if (product.DateSold != null)
                product.Sold();

            return Result.Success();
        }
    }
}
