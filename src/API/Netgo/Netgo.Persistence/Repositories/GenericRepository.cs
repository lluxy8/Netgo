using Microsoft.EntityFrameworkCore;
using Netgo.Application.Common;
using Netgo.Application.Contracts.Persistence;
using Netgo.Domain.Common;
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

        public async Task<PagedResult<T>> GetAllFilteredPaged(
            Expression<Func<T, bool>>? filter = null,
            int page = 1,
            int pageSize = 10)
        {
            IQueryable<T> query = _context.Set<T>().AsNoTracking();

            if (filter != null)
                query = query.Where(filter);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .OrderByDescending(x => x.DateCreated)
                .ToListAsync();

            return new PagedResult<T>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
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