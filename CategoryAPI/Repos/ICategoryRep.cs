using CategoryAPI.Models;

namespace CategoryAPI.Repos
{
    public interface ICategoryRep
    {
        Task<Category> GetAllCategories();
        Task<Category> GetCategoryById(Guid id);
        Task Create(Category model);
        Task Update(Category model);
        Task Delete(Category model);
        Task<bool> Save();

    }
}
