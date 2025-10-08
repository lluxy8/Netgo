using Netgo.Application.DTOs.Product;
using Netgo.Domain;
using System.Linq.Expressions;

namespace Netgo.Application.Filters
{
    public static class ProductFilter
    {
        public static Expression<Func<Product, bool>> Filter(GetProductsFilterDTO filter)
            => x =>
                (string.IsNullOrEmpty(filter.Title) || x.Title.Contains(filter.Title)) &&
                (filter.PriceMin == null || x.Price >= filter.PriceMin) &&
                (filter.PriceMax == null || x.Price <= filter.PriceMax) &&
                (filter.CategoryId == null || x.CategoryId == filter.CategoryId) &&
                (filter.PriceFixed == null || x.Price == filter.PriceFixed) &&
                (filter.Tradable == null || x.Tradable == filter.Tradable) &&
                (filter.Sold == null || (filter.Sold.Value ? x.DateSold != null : x.DateSold == null));
    }
}
