using Microsoft.EntityFrameworkCore;
using Netgo.Application.Contracts.Persistence;
using Netgo.Domain.Common;

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
            return await _context.Set<T>().AsNoTracking().ToListAsync();
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