using Microsoft.AspNetCore.Http;

namespace Netgo.Application.DTOs.Product
{
    public class CreateProductDTO
    {
        public Guid UserId { get; set; }
        public Guid CategoryId { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public bool Tradable { get; set; }
        public decimal Price { get; set; }
        public List<ProductDetailDto> Details { get; set; } 
        public List<IFormFile> Images { get; set; }
    }
}
