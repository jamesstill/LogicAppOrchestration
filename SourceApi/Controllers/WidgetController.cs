using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SourceApi.Models;
using SourceApi.Repository;

namespace SourceApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class WidgetController(WidgetContext context) : ControllerBase
    {
        private readonly WidgetContext _context = context;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<Widget> result = await _context.Widgets.ToListAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Widget widget)
        {
            _context.Widgets.Add(widget);
            await _context.SaveChangesAsync();
            return Ok(widget);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Widget widget)
        {
            _context.Widgets.Update(widget);
            await _context.SaveChangesAsync();
            return Ok(widget);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Widget widget)
        {
            _context.Widgets.Remove(widget);
            await _context.SaveChangesAsync();
            return Ok(widget);
        }
    }
}
