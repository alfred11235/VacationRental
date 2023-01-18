using System.Collections.Generic;

namespace VacationRental.Api.Resources
{
    public class CalendarOutputResource
    {
        public int RentalId { get; set; }
        public List<CalendarDateOutputResource> Dates { get; set; }
    }
}
