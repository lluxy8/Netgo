using AutoMapper;
using MediatR;
using Netgo.Application.Common;
using Netgo.Application.Contracts.Persistence;
using Netgo.Application.DTOs.Product;
using Netgo.Application.Features.Products.Requests.Query;

namespace Netgo.Application.Features.Products.Handlers.Query
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, Result<List<ListProductDTO>>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetProductsQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<Result<List<ListProductDTO>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetAll();
            var productsMapped = _mapper.Map<List<ListProductDTO>>(products);
            
            foreach(var i in productsMapped)
            {
                i.Image = products.Where(x => 
                    x.Id == i.Id)
                    .FirstOrDefault()!
                    .Images.FirstOrDefault() 
                    ?? "";
            }

            return Result<List<ListProductDTO>>.Success(productsMapped);
        }
    }
}
