using AutoMapper;
using MediatR;
using Netgo.Application.Common;
using Netgo.Application.Contracts.Identity;
using Netgo.Application.Contracts.Persistence;
using Netgo.Application.DTOs.Product;
using Netgo.Application.Exceptions;
using Netgo.Application.Features.Products.Requests.Query;
using Netgo.Application.Filters;
using Netgo.Application.Models;
using Netgo.Application.Models.Identity;
using Netgo.Domain;

namespace Netgo.Application.Features.Products.Handlers.Query
{
    public class GetUSerProductsQueryHandler : IRequestHandler<GetUserProductsQuery, Result<List<ListProductDTO>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public GetUSerProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<Result<List<ListProductDTO>>> Handle(GetUserProductsQuery request, CancellationToken cancellationToken)
        {
            var userExists = await _userService.UserExists(request.Id.ToString());

            if (!userExists)
                throw new NotFoundException(nameof(User), request.Id);

            var filter = ProductFilter.Filter(request.Filter);

            var products = await _unitOfWork.Products.GetProductsByUserIdFilteredPaged(request.Id,
                new PagedFilter<Product>
                {
                    Filter = filter,
                    PageSize = request.Filter.PageSize,
                    Page = request.Filter.Page
                });
            var productsDto = _mapper.Map<List<ListProductDTO>>(products);

            return Result<List<ListProductDTO>>.Success(productsDto);
        }
    }
}
