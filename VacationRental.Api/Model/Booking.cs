using System;

namespace VacationRental.Api.Model
{
    public class Booking
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
        public UnitInformation UnitInformation { get; set; }
    }
}
