using SmartAlertAPI.Data;
using SmartAlertAPI.Models;

namespace SmartAlertAPI.Repositories
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public DangerCategory GetCategory(string name)
        {
            return _context.DangerCategories.FirstOrDefault(c => c.Name.Equals(name))!;
        }

        public ICollection<DangerCategory> GetDangerCategories()
        {
            return _context.DangerCategories.ToList();
        }

        ICollection<string> ICategoryRepo.GetDangerCategoriesNames()
        {
            return _context.DangerCategories.Select(c => c.Name).ToList();
        }
    }
}
