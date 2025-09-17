using Microsoft.EntityFrameworkCore;
using Netgo.Application.Contracts.Persistence;
using Netgo.Domain;

namespace Netgo.Persistence.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(NetgoDbContext context) : base(context)
        {
        }

        public Task<Category?> GetCategoryByName(string name) =>
            _context.Categories.FirstOrDefaultAsync(x => x.Name == name);

        public Task<bool> CategoryExistsByName(string name) =>
            _context.Categories.AnyAsync(x => x.Name == name);

        public Task<Category?> GetCategoryWithProducts(Guid id) => 
            _context.Categories.Include(x => x.Products).FirstOrDefaultAsync(x => x.Id == id);

    }
}
