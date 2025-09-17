using Netgo.Domain.Common;

namespace Netgo.Domain
{
    public class ProductDetail : DomainEntity
    {
        public Guid ProductId { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
        public Product Product { get; set; }
    }
}
