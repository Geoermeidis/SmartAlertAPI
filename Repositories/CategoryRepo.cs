using Microsoft.EntityFrameworkCore;
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
        public async Task<DangerCategory> GetCategory(string name)
        {
            return await _context.DangerCategories.FirstOrDefaultAsync(c => c.Name.Equals(name))!;
        }

        public async Task<ICollection<DangerCategory>> GetDangerCategories()
        {
            return await _context.DangerCategories.ToListAsync();
        }

        public async Task<ICollection<string>> GetDangerCategoriesNames()
        {
            return await _context.DangerCategories.Select(c => c.Name).ToListAsync();
        }
    }
}
