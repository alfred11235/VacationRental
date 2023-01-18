using Microsoft.AspNetCore.Mvc;
using System;
using VacationRental.Api.IRepositories;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly IRentalsRepository _rentalsRepository;

        public CalendarController(IRentalsRepository rentalsRepository)
        {
            _rentalsRepository = rentalsRepository;
        }

        [HttpGet]
        public IActionResult Get(int rentalId, DateTime start, int nights)
        {
            if (nights < 0)
                return BadRequest("Nights must be positive");
            try
            {
                var result = _rentalsRepository.GetCalendar(rentalId, start, nights);
                if(result == null)
                    return BadRequest("Rental not found");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
