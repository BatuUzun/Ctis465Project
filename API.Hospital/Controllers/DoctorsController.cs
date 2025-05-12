#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediatR;
using CORE.APP.Features;
using APP.Hospital.Features.Doctors;
using Microsoft.AspNetCore.Authorization;

//Generated from Custom Template.
namespace API.Hospital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class DoctorsController : ControllerBase
    {
        private readonly ILogger<DoctorsController> _logger;
        private readonly IMediator _mediator;

        public DoctorsController(ILogger<DoctorsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        // GET: api/Doctors
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var response = await _mediator.Send(new DoctorQueryRequest());
                var list = await response.ToListAsync();
                if (list.Any())
                    return Ok(list);
                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError("DoctorsGet Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during DoctorsGet.")); 
            }
        }

        // GET: api/Doctors/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var response = await _mediator.Send(new DoctorQueryRequest());
                var item = await response.SingleOrDefaultAsync(r => r.Id == id);
                if (item is not null)
                    return Ok(item);
                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError("DoctorsGetById Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during DoctorsGetById.")); 
            }
        }

		// POST: api/Doctors
        [HttpPost]
        public async Task<IActionResult> Post(DoctorCreateRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await _mediator.Send(request);
                    if (response.IsSuccessful)
                    {
                        //return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
                        return Ok(response);
                    }
                    ModelState.AddModelError("DoctorsPost", response.Message);
                }
                return BadRequest(new CommandResponse(false, string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
            }
            catch (Exception exception)
            {
                _logger.LogError("DoctorsPost Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during DoctorsPost.")); 
            }
        }

        // PUT: api/Doctors
        [HttpPut]
        public async Task<IActionResult> Put(DoctorUpdateRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await _mediator.Send(request);
                    if (response.IsSuccessful)
                    {
                        //return NoContent();
                        return Ok(response);
                    }
                    ModelState.AddModelError("DoctorsPut", response.Message);
                }
                return BadRequest(new CommandResponse(false, string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
            }
            catch (Exception exception)
            {
                _logger.LogError("DoctorsPut Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during DoctorsPut.")); 
            }
        }

        // DELETE: api/Doctors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _mediator.Send(new DoctorDeleteRequest() { Id = id });
                if (response.IsSuccessful)
                {
                    //return NoContent();
                    return Ok(response);
                }
                ModelState.AddModelError("DoctorsDelete", response.Message);
                return BadRequest(new CommandResponse(false, string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
            }
            catch (Exception exception)
            {
                _logger.LogError("DoctorsDelete Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during DoctorsDelete.")); 
            }
        }
	}
}
