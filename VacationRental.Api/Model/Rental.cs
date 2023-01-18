using System.Collections.Generic;

namespace VacationRental.Api.Model
{
    public class Rental
    {
        public int Id { get; set; }
        public int Units { get; set; }

        public int PreparationTimeInDays { get; set; }

        public List<UnitInformation> UnitsInformation { get; set; } = new List<UnitInformation>();
    }
}
