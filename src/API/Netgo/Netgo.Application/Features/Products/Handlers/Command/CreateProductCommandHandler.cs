using AutoMapper;
using MediatR;
using Netgo.Application.Common;
using Netgo.Application.Contracts.Identity;
using Netgo.Application.Contracts.Infrastructure;
using Netgo.Application.Contracts.Persistence;
using Netgo.Application.DTOs.Product.Validators;
using Netgo.Application.Exceptions;
using Netgo.Application.Features.Products.Requests.Command;
using Netgo.Domain;

namespace Netgo.Application.Features.Products.Handlers.Command
{
    public class CreatePRoductCommandHandler : IRequestHandler<CreateProductCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;


        public CreatePRoductCommandHandler(
            IUnitOfWork _unitOFWork, 
            IMapper mapper,
            IFileService fileService,
            IEmailService emailService,
            IUserService userService)
        {
            _unitOfWork = _unitOFWork;
            _mapper = mapper;
            _fileService = fileService;
            _emailService = emailService;
            _userService = userService;
        }

        public async Task<Result<Guid>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateProductDTOValidator();
            var validationResult = await validator.ValidateAsync(request.ProductDto, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult);

            var categoryExists = await _unitOfWork.Categories.Exists(request.ProductDto.CategoryId);
            if (!categoryExists)
                throw new NotFoundException("Category", request.ProductDto.CategoryId);


            var user = await _userService.GetUser(request.ProductDto.UserId.ToString());
            if(user is null)
                throw new NotFoundException(nameof(user), request.ProductDto.UserId);

            var product = _mapper.Map<Product>(request.ProductDto);
            product.Images = [];
            product.Details = _mapper.Map<List<ProductDetail>>(request.ProductDto.Details);
            foreach (var detail in product.Details)
            {
                detail.ProductId = product.Id;
            }

            foreach (var image in request.ProductDto.Images)
            {
                string path = await _fileService.SaveFileAsync(product.Id.ToString(), image);
                product.Images.Add(path);
            }

            await _unitOfWork.Products.Insert(product);

            await _emailService.Send(new Models.Email
            {
                To = user.Email,
                Body = $"""
                    You created a new product "{product.Title}". 
                """,
                Subject = "Product created successfully."
            });

            return Result<Guid>.Success(product.Id);
        }
    }
}
    