using System;

namespace VacationRental.Api.Resources
{
    public class BookingOutputResource
    {
        public int Id { get; set; }
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
    }
}
