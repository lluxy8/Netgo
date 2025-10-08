using Microsoft.EntityFrameworkCore;
using Netgo.Application.Common;
using Netgo.Application.Contracts.Persistence;
using Netgo.Application.Models;
using Netgo.Domain;
using Netgo.Persistence.Helper;
using System.Linq;

namespace Netgo.Persistence.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    { 
        public ProductRepository(NetgoDbContext context) : base(context)
        {
        }

        public async Task<PagedResult<Product>> GetProductsByUserIdFilteredPaged(
            Guid userId, PagedFilter<Product> filter)
        {
            IQueryable<Product> query = _context
                .Products
                .Where(x => x.UserId == userId)
                .AsNoTracking();

            if (filter.Filter != null)
                query = query.Where(filter.Filter);

            var totalCount = await query.CountAsync();
            Queries.GetPaged<Product>(query, filter.Page, filter.PageSize);
            var items = await query
                .OrderByDescending(x => x.DateCreated)
                .ToListAsync();

            int shownCount = filter.Page * filter.PageSize;
            int remainingItems = totalCount - shownCount;
            if (remainingItems < 0)
                remainingItems = 0;

            return new PagedResult<Product>
            {
                Items = items,
                TotalCount = totalCount,
                RemainingCount = remainingItems,
                Page = filter.Page,
                PageSize = filter.PageSize
            };
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
