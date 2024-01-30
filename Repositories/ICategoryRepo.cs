using SmartAlertAPI.Models;

namespace SmartAlertAPI.Repositories
{
    public interface ICategoryRepo
    {
        Task<ICollection<DangerCategory>> GetDangerCategories();
        Task<ICollection<string>> GetDangerCategoriesNames();
        Task<DangerCategory> GetCategory(string name);
    }
}
