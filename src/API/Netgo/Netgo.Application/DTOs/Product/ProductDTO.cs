namespace Netgo.Application.DTOs.Product
{
    public class ProductDTO
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string Title { get; set; }
        public string NormalizedTitle { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool Tradable { get; set; }
        public DateTime? DateSold { get; set; }
        public List<ProductDetailDto> Details { get; set; }
        public List<string> Images { get; set; }
        public DateTime? DateArchived { get; set; } = null;
    }
}
