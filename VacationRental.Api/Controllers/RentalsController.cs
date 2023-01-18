using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using VacationRental.Api.IRepositories;
using VacationRental.Api.Model;
using VacationRental.Api.Resources;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRentalsRepository _rentalsRepository;

        public RentalsController(IMapper mapper,
                                 IRentalsRepository rentalsRepository)
        {
            _mapper = mapper;
            _rentalsRepository = rentalsRepository;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public IActionResult Get(int rentalId)
        {
            try
            {
                var result = _rentalsRepository.GetRental(rentalId);
                if(result == null)
                    BadRequest("Rental not found");
                return Ok(_mapper.Map<Rental, RentalOutputResource>(result));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public IActionResult Post(RentalInputResource model)
        {
            try
            {
                return Ok(_rentalsRepository.PostRental(model));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpPut]
        [Route("{rentalId:int}")]
        public IActionResult Put(int rentalId, RentalInputResource model)
        {
            try
            {
                var result = _rentalsRepository.PutRental(rentalId, model);
                if (result == null)
                    return BadRequest("There are bookings that prevent the update of the parameters");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}
