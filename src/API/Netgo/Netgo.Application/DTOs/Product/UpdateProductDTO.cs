using Microsoft.AspNetCore.Http;

namespace Netgo.Application.DTOs.Product
{
    public class UpdateProductDTO
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public bool Tradable { get; set; }
        public List<ProductDetailDto> Details { get; set; }
        public List<IFormFile> NewImages { get; set; }
        public List<string> Images { get; set; }
        public bool Archieved { get; set; }
    }
}
