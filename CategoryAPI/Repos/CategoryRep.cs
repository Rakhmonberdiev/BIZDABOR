using CategoryAPI.Data;
using CategoryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CategoryAPI.Repos
{
    public class CategoryRep : ICategoryRep
    {
        private readonly CategoryDb _db;
        public CategoryRep(CategoryDb db)
        {
            _db = db;
        }
        public async Task Create(Category model)
        {
            await _db.Categories.AddAsync(model);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(Category model)
        {
            _db.Categories.Remove(model);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> ExistingCategory(string title)
        {
            return await _db.Categories.AnyAsync(x => x.Title == title);
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await _db.Categories.OrderBy(c => c.Title).ToListAsync();
        }

        public async Task<Category> GetCategoryById(Guid id)
        {
            return await _db.Categories.SingleOrDefaultAsync(x=>x.Id == id);
        }

        public async Task Update(Category model)
        {
            _db.Entry(model).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }
    }
}
