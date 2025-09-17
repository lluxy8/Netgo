using AutoMapper;
using MediatR;
using Netgo.Application.Common;
using Netgo.Application.Contracts.Persistence;
using Netgo.Application.DTOs.Product;
using Netgo.Application.Exceptions;
using Netgo.Application.Features.Products.Requests.Query;
using Netgo.Domain;

namespace Netgo.Application.Features.Products.Handlers.Query
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Result<ProductDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<ProductDTO>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Products.GetProductWithDetails(request.Id)
                ?? throw new NotFoundException(nameof(Product), request.Id);

            var productDto = _mapper.Map<ProductDTO>(product);
            var productDetailsDto = product.Details != null 
                ? _mapper.Map<List<ProductDetailDto>>(product.Details) 
                : [];

            return Result<ProductDTO>.Success(productDto);
        }
    }
}
