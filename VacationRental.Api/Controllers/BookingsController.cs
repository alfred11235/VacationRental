using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using VacationRental.Api.IRepositories;
using VacationRental.Api.Model;
using VacationRental.Api.Resources;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRentalsRepository _rentalsRepository;

        public BookingsController(IMapper mapper, 
                                  IRentalsRepository rentalsRepository)
        {
            _mapper = mapper;
            _rentalsRepository = rentalsRepository;
        }

        [HttpGet]
        [Route("{bookingId:int}")]
        public IActionResult Get(int bookingId)
        {
            try
            {
                var result = _rentalsRepository.GetBooking(bookingId);
                if(result == null)
                    return BadRequest("Booking not found");
                return Ok(_mapper.Map<Booking, BookingOutputResource>(result));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public IActionResult Post(BookingInputResource model)
        {
            if (model.Nights <= 0)
                return BadRequest("Nigts must be positive");
            try
            {
                var result = _rentalsRepository.PostBooking(model);
                if (result == null)
                    return BadRequest("Not available");
                if (result.Id == 0)
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
