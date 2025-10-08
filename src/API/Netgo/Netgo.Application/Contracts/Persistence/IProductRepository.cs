using Netgo.Application.Common;
using Netgo.Application.Models;
using Netgo.Domain;

namespace Netgo.Application.Contracts.Persistence
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<List<Product>> GetProductsByUserId(Guid userId);
        Task<PagedResult<Product>> GetProductsByUserIdFilteredPaged(
            Guid userId, PagedFilter<Product> filter);
        Task<Product?> GetProductWithDetails(Guid productId);
    }
}
