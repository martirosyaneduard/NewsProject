using Microsoft.EntityFrameworkCore;
using NewsProject.DataTransferObject;
using NewsProject.Models;
using System.ComponentModel.DataAnnotations;

namespace NewsProject.Services
{
    public class NewService:IService<New,NewDto>,ISearchService
    {
        private readonly NewsDbContext _context;

        public NewService(NewsDbContext context)
        {
            this._context = context;
        }
        public async Task<New> Add(NewDto entity)
        {
            List<Category> categories = new();
            foreach (var categoryId in entity.CategoriesId)
            {
                var category = await _context.Categories.FindAsync(categoryId);
                if (category is null)
                {
                    continue;
                }
                categories.Add(category);
            }
            var newobject = new New()
            {
                Title = entity.Title,
                Content = entity.Content,
                CreatedAt = entity.CreatedAt,
                Categories = categories
            };
            _context.News.Add(newobject);
            await _context.SaveChangesAsync();
            return newobject;
        }
        public async Task Delete(int id)
        {
            var newobject = await GetById(id);
            _context.News.Remove(newobject);

            await _context.SaveChangesAsync();
        }
        public async IAsyncEnumerable<New> GetAll()
        {
            var news = _context.News
                .AsNoTracking()
                .Include(x => x.Categories)
                .AsAsyncEnumerable();
            await foreach (var newitem in news)
            {
                yield return newitem;
            }
        }
        public async Task<New> GetById([Range(0, int.MaxValue)] int id)
        {
            var newobject = await _context.News
                .AsNoTracking()
                .Include(x => x.Categories)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (newobject is null)
            {
                throw new ArgumentException("Id is not found");
            }
            return newobject;
        }
        public async Task Update(NewDto entity, [Range(0, int.MaxValue)] int id)
        {
            var newobject = await _context.News.Include(c => c.Categories).FirstOrDefaultAsync(g => g.Id == id);
            if (newobject is null)
            {
                throw new ArgumentException("Id is not found");
            }

            newobject.Categories.Where(category => !entity.CategoriesId.Any(id => id == category.Id)).ToList().ForEach(category => newobject.Categories.Remove(category));
            entity.CategoriesId.Where(id => !newobject.Categories.Any(caetgory => id == caetgory.Id)).ToList().ForEach(id => newobject.Categories.Add(new Category { Id = id }));

            newobject.Title = entity.Title;
            newobject.Content = entity.Content;
            newobject.CreatedAt= entity.CreatedAt;
            await _context.SaveChangesAsync();
        }

        public async IAsyncEnumerable<New> SearchWithDate(DateDto entity)
        {
            var news = _context.News
                       .AsNoTracking()
                       .Include(n => n.Categories)
                       .Where(n => n.CreatedAt > entity.From && n.CreatedAt < entity.To)
                       .AsAsyncEnumerable();
            await foreach (var n in news)
            {
                yield return n;
            }
        }

        public async IAsyncEnumerable<New> SearchWithText(string text)
        {
            var news = _context.News
                       .AsNoTracking()
                       .Where(n => n.Title.Contains(text) || n.Content.Contains(text))
                       .AsAsyncEnumerable();
            await foreach (var n in news)
            {
                yield return n;
            }
        }
    }
}
