using Netgo.Domain;

namespace Netgo.Application.Contracts.Persistence
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<List<Product>> GetProductsByUserId(Guid userId);
        Task<Product?> GetProductWithDetails(Guid productId);
    }
}
