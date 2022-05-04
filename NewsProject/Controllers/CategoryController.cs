using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsProject.DataTransferObject;
using NewsProject.Models;
using NewsProject.Services;

namespace NewsProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IService<Category, CategoryDto> _service;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(IService<Category, CategoryDto> service, ILogger<CategoryController> logger)
        {
            _service = service;
            _logger = logger;
        }
        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK,
            Type = typeof(IAsyncEnumerable<Category>))]
        public IActionResult GetAll()
        {
            var category = _service.GetAll();
            return Ok(category);
        }
        [HttpGet("{id}", Name = nameof(GetCategoryById))]
        [ProducesResponseType(StatusCodes.Status200OK,
            Type = typeof(Category))]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _service.GetById(id);
            return Ok(category);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Post(CategoryDto entity)
        {
            var category = await _service.Add(entity);
            _logger.LogInformation($"Added new category {category.Id}");

            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Put(int id, CategoryDto entity)
        {
            await _service.Update(entity, id);
            _logger.LogInformation("Updated category {id}", id);
            return NoContent();
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.Delete(id);
            _logger.LogInformation("Deleted category {id}", id);
            return NoContent();
        }
    }
}
