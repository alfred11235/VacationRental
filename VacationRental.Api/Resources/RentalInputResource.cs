using System.Collections.Generic;
using VacationRental.Api.Model;

namespace VacationRental.Api.Resources
{
    public class RentalInputResource
    {
        public int Units { get; set; }

        public int PreparationTimeInDays { get; set; }

        public List<string> UnitsDescriptions{ get; set; } = new List<string>();
    }
}
