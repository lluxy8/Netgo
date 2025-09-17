namespace Netgo.Application.DTOs.Product
{
    public class ListProductDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? DateSold { get; set; }
        public bool Tradable { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
    }
}
