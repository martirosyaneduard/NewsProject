using Microsoft.EntityFrameworkCore;
using NewsProject.DataTransferObject;
using NewsProject.Models;
using System.ComponentModel.DataAnnotations;

namespace NewsProject.Services
{
    public class CategoryService:IService<Category,CategoryDto>
    {
        private readonly NewsDbContext _context;

        public CategoryService(NewsDbContext context)
        {
            this._context = context;
        }
        public async Task<Category> Add(CategoryDto entity)
        {
            var category = new Category();
            category.Name= entity.Name;
            _context?.Categories?.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }
        public async Task Delete(int id)
        {
            var category = await GetById(id);
            _context.Categories.Remove(category);

            await _context.SaveChangesAsync();
        }
        public async IAsyncEnumerable<Category> GetAll()
        {
            var categories = _context.Categories
                .AsNoTracking()
                .Include(newobj => newobj.News)
                .AsAsyncEnumerable();
            await foreach (var caetgory in categories)
            {
                yield return caetgory;
            }
        }
        public async Task<Category> GetById([Range(0, int.MaxValue)] int id)
        {
            var category = await _context.Categories
                .AsNoTracking()
                .Include(newobj => newobj.News)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (category is null)
            {
                throw new ArgumentException("Id is not found");
            }
            return category;
        }
        public async Task Update(CategoryDto entity, [Range(0, int.MaxValue)] int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(g => g.Id == id);
            if (category is null)
            {
                throw new ArgumentException("Id is not found");
            }
            category.Name = entity.Name;
            await _context.SaveChangesAsync();
        }
    }
}
