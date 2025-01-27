using Microsoft.AspNetCore.Mvc;
using Million.Technical.Test.Application.Commands;
using Million.Technical.Test.Libraries.Mediators;

namespace Million.Technical.Test.Api.Controllers
{
    [ApiController]
    [Route("api/owner")]
    [Produces("application/json")]
    public class OwnerController : ControllerBase
    {
        private readonly Mediator _mediator;

        public OwnerController(Mediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create_owner")]
        public async Task<ActionResult<Guid>> CreateOwner(
            [FromForm] CreateOwnerCommand command)
        {
            try
            {
                var ownerId = await _mediator.SendAsync<CreateOwnerCommand, Guid>(command);
                return ownerId;
            }
            catch (FluentValidation.ValidationException ex)
            {
                return BadRequest(ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred while creating the owner");
            }
        }
    }
}