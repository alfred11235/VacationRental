using System.Collections.Generic;

namespace VacationRental.Api.Model
{
    public class UnitInformation
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string Description { get; set; }
        public Rental Rental { get; set; }
        public List<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
