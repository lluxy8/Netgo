using AutoMapper;
using MediatR;
using Netgo.Application.Common;
using Netgo.Application.Contracts.Identity;
using Netgo.Application.Contracts.Persistence;
using Netgo.Application.DTOs.Product;
using Netgo.Application.Exceptions;
using Netgo.Application.Features.Products.Requests.Query;

namespace Netgo.Application.Features.Products.Handlers.Query
{
    public class GetProductWİthOwnerQueryHandler : IRequestHandler<GetProductWithOwnerQuery, Result<ProductWithOwnerDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public GetProductWİthOwnerQueryHandler(IUnitOfWork unitOfWork, IUserService userService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<Result<ProductWithOwnerDTO>> Handle(GetProductWithOwnerQuery request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Products.GetProductWithDetails(request.ProductId)
                ?? throw new NotFoundException("Product", request.ProductId);
            var user = await _userService.GetUser(product.UserId.ToString())
                ?? throw new NotFoundException("User", product.UserId);

            var productWOwner = new ProductWithOwnerDTO
            {
                OwnerId = user.Id,
                ProductId = product.Id,
                ProductDescription = product.Description,
                ProductImages = product.Images,
                ProductTitle = product.Title,
                ProductDetails = _mapper.Map<List<ProductDetailDto>>(product.Details),
                OwnerName = user.FirstName + " " + user.LastName,
                OwnerVerifiedSeller = user.VerifiedSeller,
                OwnerAvatar = user.ProfilePictureURL,
                OwnerLocation = user.Location
            };

            return Result<ProductWithOwnerDTO>.Success(productWOwner);          
        }
    }
}
