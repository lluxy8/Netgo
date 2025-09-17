using Netgo.Domain.Common;

namespace Netgo.Domain
{
    public class Category : DomainEntity
    {
        public string Name { get; set; }
        public List<Product> Products { get; set; }
    }
}
