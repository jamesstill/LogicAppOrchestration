using Microsoft.AspNetCore.Mvc;
using TargetApi.Models;
using TargetApi.Repository;

namespace TargetApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PayloadController(IPayloadRepository repository) : ControllerBase
    {
        private readonly IPayloadRepository _repository = repository;

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]PayloadDto payload)
        {
            try
            {
                ReturnDto returnDto = await _repository.PostPayload(payload);
                return Ok(returnDto);
            }
            catch (Exception ex)
            {
                var details = new ProblemDetails
                {
                    Title = "Server Error",
                    Status = StatusCodes.Status409Conflict,
                    Detail = ex.Message,
                    Instance = HttpContext.Request.Path
                };

                return Conflict(details);
            }
        }
    }
}
