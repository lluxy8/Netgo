using Microsoft.EntityFrameworkCore;
using Netgo.Application.Contracts.Persistence;
using Netgo.Domain;

namespace Netgo.Persistence.Repositories
{
    public class ProductDetailRepository : GenericRepository<ProductDetail>, IProductDetailRepository
    {
        private readonly NetgoDbContext _context;
        public ProductDetailRepository(NetgoDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<ProductDetail>> GetProductDetailsByProductId(Guid productId)
            => _context.ProductDetails.Where(pd => pd.ProductId == productId).ToListAsync();
    }
}
