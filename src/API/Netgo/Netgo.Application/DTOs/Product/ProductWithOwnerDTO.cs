namespace Netgo.Application.DTOs.Product
{
    public class ProductWithOwnerDTO
    {
        public Guid ProductId { get; set; }
        public string ProductTitle { get; set; }
        public string ProductDescription { get; set; }
        public List<string> ProductImages { get; set; }
        public List<ProductDetailDto> ProductDetails { get; set; }
        public string OwnerId { get; set; }
        public string OwnerName { get; set; }
        public string OwnerLocation { get; set; }
        public string OwnerAvatar { get; set; }
        public bool OwnerVerifiedSeller { get; set; }
    }
}
