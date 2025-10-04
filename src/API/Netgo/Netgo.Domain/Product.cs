using Netgo.Domain.Common;
using Netgo.Domain.Events.Products;

namespace Netgo.Domain
{
    public class Product : DomainEntity
    {
        public Guid CategoryId { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string NormalizedTitle { get; set; }
        public string Description { get; set; }
        public bool Tradable { get; set; }
        public decimal Price { get; set; }
        public DateTime? DateSold { get; set; } = null;
        public Category Category { get; set; }
        public List<ProductDetail> Details { get; set; }
        public List<string> Images { get; set; }

        public Product() { }

        public void Created() => AddDomainEvent(new ProductCreatedEvent(this));
        public void Updated(Product oldProduct) => AddDomainEvent(new ProductUpdatedEvent(oldProduct, this));
        public void Sold() => AddDomainEvent(new ProductSoldEvent(this));
    }
}
