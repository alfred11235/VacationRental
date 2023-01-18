using AutoMapper;
using VacationRental.Api.Model;
using VacationRental.Api.Resources;

namespace QRAccessService.Mapping
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<Rental, RentalOutputResource>();
            CreateMap<Booking, BookingOutputResource>()
                .ForMember(dest => dest.RentalId, opt => opt.MapFrom(src => src.UnitInformation.Rental.Id));
        }
    }
}
