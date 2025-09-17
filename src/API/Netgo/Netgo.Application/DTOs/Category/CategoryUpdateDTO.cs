namespace Netgo.Application.DTOs.Category
{
    public class CategoryUpdateDTO
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
    }
}
