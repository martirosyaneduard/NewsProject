using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsProject.DataTransferObject;
using NewsProject.Models;
using NewsProject.Services;

namespace NewsProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly IService<New, NewDto> _service;
        private readonly ILogger<NewsController> _logger;
        private readonly ISearchService _searchService;

        public NewsController(IService<New, NewDto> service,ISearchService searchService, ILogger<NewsController> logger)
        {
            _service = service;
            _logger = logger;
            this._searchService = searchService;
        }
        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK,
            Type = typeof(IAsyncEnumerable<New>))]
        public IActionResult GetAll()
        {
            var newsobject = _service.GetAll();
            return Ok(newsobject);
        }
        [HttpGet("{id}", Name = nameof(GetNewsById))]
        [ProducesResponseType(StatusCodes.Status200OK,
            Type = typeof(New))]
        public async Task<IActionResult> GetNewsById(int id)
        {
            var newobject = await _service.GetById(id);
            return Ok(newobject);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Post(NewDto entity)
        {
            var @new = await _service.Add(entity);
            _logger.LogInformation($"Added new news {@new.Id}");

            return CreatedAtAction(nameof(GetNewsById), new { id = @new.Id }, @new);
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Put(int id, NewDto entity)
        {
            await _service.Update(entity, id);
            _logger.LogInformation("Updated news {id}", id);
            return NoContent();
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.Delete(id);
            _logger.LogInformation("Deleted news {id}", id);
            return NoContent();
        }

        [HttpPost("SearchWithDate")]//get ov arel em exceptiona tvel
        [ProducesResponseType(StatusCodes.Status200OK,
           Type = typeof(IAsyncEnumerable<New>))]
        public IActionResult SearchByDate(DateDto date)
        {
            var news = _searchService.SearchWithDate(date);
            return Ok(news);
        }

        [HttpPost("SearchWithText")]//get ov arel em exceptiona tvel
        [ProducesResponseType(StatusCodes.Status200OK,
            Type = typeof(IAsyncEnumerable<New>))]
        public IActionResult SearchByText(string text)
        {
            var news = _searchService.SearchWithText(text);
            return Ok(news);
        }
    }
}
