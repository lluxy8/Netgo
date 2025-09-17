using Netgo.Domain.Common;

namespace Netgo.Application.Contracts.Persistence
{
    public interface IGenericRepository<T> where T : DomainEntity
    {
        Task<T?> GetById(Guid Id);
        Task<IReadOnlyList<T>> GetAll();
        Task<bool> Exists(Guid Id);
        Task Insert(T entity);
        Task Update(T entity);
        Task Delete(T entity);
    }
}
