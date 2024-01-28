using SmartAlertAPI.Models;

namespace SmartAlertAPI.Repositories
{
    public interface ICategoryRepo
    {
        ICollection<DangerCategory> GetDangerCategories();
        ICollection<string> GetDangerCategoriesNames();
        DangerCategory GetCategory(string name);
    }
}
