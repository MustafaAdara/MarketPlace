using Marketplace.Models;

namespace Marketplace.Interfaces
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();

        Category GetCategory(int id);
        Category GetCategoryOfAProduct(int categoryId);
        bool CategoriesExists(int id);
        bool CreateCategory(Category category);
        bool UpdateCategory(Category category);
        bool DeleteCategory(Category category);
        bool Save();
    }
}
