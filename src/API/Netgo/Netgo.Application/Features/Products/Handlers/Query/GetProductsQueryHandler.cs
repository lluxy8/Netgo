using AutoMapper;
using MediatR;
using Netgo.Application.Common;
using Netgo.Application.Contracts.Persistence;
using Netgo.Application.DTOs.Product;
using Netgo.Application.Exceptions;
using Netgo.Application.Features.Products.Requests.Query;
using Netgo.Domain;
using System.Linq.Expressions;

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
            Expression<Func<Product, bool>> filter = x =>
                (string.IsNullOrEmpty(request.Filter.Title) || x.Title.Contains(request.Filter.Title)) &&
                (request.Filter.PriceMin == null || x.Price >= request.Filter.PriceMin) &&
                (request.Filter.PriceMax == null || x.Price <= request.Filter.PriceMax) &&
                (request.Filter.CategoryId == null || x.CategoryId == request.Filter.CategoryId) &&
                (request.Filter.PriceFixed == null || x.Price == request.Filter.PriceFixed) &&
                (request.Filter.Tradable == null || x.Tradable == request.Filter.Tradable) &&
                (request.Filter.Sold == null || (request.Filter.Sold.Value ? x.DateSold != null : x.DateSold == null));

            var products = await _productRepository.GetAllFilteredPaged(
                filter: filter,
                pageSize: request.Filter.PageSize,
                page: request.Filter.Page);

            if (products.TotalCount == 0)
                throw new NotFoundException("Products", "with filter");

            var productsMapped = _mapper.Map<List<ListProductDTO>>(products.Items);
            
            foreach(var i in productsMapped)
            {
                i.Image = products.Items.Where(x => 
                    x.Id == i.Id)
                    .FirstOrDefault()!
                    .Images.FirstOrDefault() 
                    ?? "";
            }

            return Result<List<ListProductDTO>>.Success(productsMapped);
        }
    }
}
