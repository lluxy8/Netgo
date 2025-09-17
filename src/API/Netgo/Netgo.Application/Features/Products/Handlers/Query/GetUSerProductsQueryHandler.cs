using AutoMapper;
using MediatR;
using Netgo.Application.Common;
using Netgo.Application.Contracts.Identity;
using Netgo.Application.Contracts.Persistence;
using Netgo.Application.DTOs.Product;
using Netgo.Application.Exceptions;
using Netgo.Application.Features.Products.Requests.Query;
using Netgo.Application.Models.Identity;

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


            var products = await _unitOfWork.Products.GetProductsByUserId(request.Id);
            var productsDto = _mapper.Map<List<ListProductDTO>>(products);

            return Result<List<ListProductDTO>>.Success(productsDto);
        }
    }
}
