using Microsoft.AspNetCore.Mvc;
using TargetApi.Repository;

namespace TargetApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class WidgetController(IWidgetRepository repository) : ControllerBase
    {
        private readonly IWidgetRepository _repository = repository;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var list = await _repository.GetAllWidgets();
            return Ok(list);
        }
    }
}
