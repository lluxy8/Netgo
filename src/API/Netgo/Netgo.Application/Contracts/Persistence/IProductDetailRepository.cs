using Netgo.Domain;

namespace Netgo.Application.Contracts.Persistence
{
    public interface IProductDetailRepository : IGenericRepository<ProductDetail>
    {
        Task<List<ProductDetail>> GetProductDetailsByProductId(Guid productId);
    }
}
