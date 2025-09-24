using Microsoft.EntityFrameworkCore;
using Netgo.Application.Contracts.Persistence;
using Netgo.Domain;

namespace Netgo.Persistence.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    { 
        public ProductRepository(NetgoDbContext context) : base(context)
        {
        }

        public Task<List<Product>> GetProductsByUserId(Guid userId)
            => _context.Products
                .Where(p => p.UserId == userId)
                .ToListAsync();

        public Task<Product?> GetProductWithDetails(Guid productId)
            => _context.Products
                .Include(p => p.Details)
                .FirstOrDefaultAsync(p => p.Id == productId);
    }
}
