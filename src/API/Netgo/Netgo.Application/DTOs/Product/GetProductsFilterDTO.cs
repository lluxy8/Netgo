using Netgo.Application.Common;

namespace Netgo.Application.DTOs.Product
{
    public class GetProductsFilterDTO : PagedRequest
    {
        public string? Title { get; set; }
        public Guid? CategoryId { get; set; }
        public decimal? PriceMin { get; set; }
        public decimal? PriceMax { get; set; }
        public decimal? PriceFixed { get; set; }
        public bool? Tradable { get; set; }
        public bool? Sold { get; set; }
    }
}
