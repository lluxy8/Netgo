using Netgo.Application.Common;
using Netgo.Domain.Common;
using System.Linq.Expressions;

namespace Netgo.Application.Contracts.Persistence
{
    public interface IGenericRepository<T> where T : DomainEntity
    {
        Task<T?> GetById(Guid Id);
        Task<IReadOnlyList<T>> GetAll();
        Task<PagedResult<T>> GetAllFilteredPaged(
            Expression<Func<T, bool>>? filter = null,
            int page = 1,
            int pageSize = 10);
        Task<bool> Exists(Guid Id);
        Task Insert(T entity);
        Task Update(T entity);
        Task Delete(T entity);
    }
}
