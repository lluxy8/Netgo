using Netgo.Domain;

namespace Netgo.Application.Contracts.Persistence
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<Category?> GetCategoryByName(string name);
        Task<bool> CategoryExistsByName(string name);
        Task<Category?> GetCategoryWithProducts(Guid id);
    }
}
