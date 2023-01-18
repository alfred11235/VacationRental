using System;
using System.Collections.Generic;

namespace VacationRental.Api.Resources
{
    public class CalendarDateOutputResource
    {
        public DateTime Date { get; set; }
        public List<CalendarBookingOutputResource> Bookings { get; set; } = new List<CalendarBookingOutputResource>();

        public List<UnitOutputResource> PreparationTimes { get; set; } = new List<UnitOutputResource>();
    }
}
