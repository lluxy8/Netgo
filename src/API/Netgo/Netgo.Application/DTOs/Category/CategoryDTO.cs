using Netgo.Application.DTOs.Product;

namespace Netgo.Application.DTOs.Category
{
    public class CategoryDTO
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required List<ProductDTO> Products { get; set; }
    }
}
