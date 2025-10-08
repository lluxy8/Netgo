using Microsoft.EntityFrameworkCore;
using Netgo.Application.Common;
using Netgo.Application.Contracts.Persistence;
using Netgo.Application.Models;
using Netgo.Domain.Common;
using Netgo.Persistence.Helper;
using System.Linq;
using System.Linq.Expressions;

namespace Netgo.Persistence.Repositories
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : DomainEntity
    {
        protected readonly NetgoDbContext _context;

        protected GenericRepository(NetgoDbContext context)
        {
            _context = context;
        }

        public async Task Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Exists(Guid Id)
        {
            return await _context.Set<T>().AnyAsync(e => e.Id == Id);
        }

        public async Task<IReadOnlyList<T>> GetAll()
        {
            return await _context.Set<T>()
                .AsNoTracking()
                .OrderByDescending(x => x.DateCreated)
                .ToListAsync();
        }

        public async Task<PagedResult<T>> GetAllFilteredPaged(PagedFilter<T> filter)
        {
            IQueryable<T> query = _context.Set<T>().AsNoTracking();

            if (filter.Filter != null)
                query = query.Where(filter.Filter);

            var totalCount = await query.CountAsync();
            Queries.GetPaged<T>(query, filter.Page, filter.PageSize);
            var items = await query
                .OrderByDescending(x => x.DateCreated)
                .ToListAsync();

            int shownCount = filter.Page * filter.PageSize;
            int remainingItems = totalCount - shownCount;
            if (remainingItems < 0)
                remainingItems = 0;

            return new PagedResult<T>
            {
                Items = items,
                TotalCount = totalCount,
                RemainingCount = remainingItems,
                Page = filter.Page,
                PageSize = filter.PageSize
            };
        }

        public async Task<T?> GetById(Guid Id)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(e => e.Id == Id);
        }

        public async Task Insert(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}