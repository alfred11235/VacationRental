using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Api.IRepositories;
using VacationRental.Api.Model;
using VacationRental.Api.Resources;

namespace VacationRental.Api.Repositories
{
    public class RentalsRepository : IRentalsRepository
    {
        private readonly IDictionary<int, Rental> _rentals;
        private readonly IDictionary<int, Booking> _bookings;
        private readonly IDictionary<int, UnitInformation> _unitsInfo;

        public RentalsRepository(IDictionary<int, Rental> rentals,
                                 IDictionary<int, Booking> bookings,
                                 IDictionary<int, UnitInformation> unitsInfo)
        {
            _rentals = rentals;
            _bookings = bookings;
            _unitsInfo = unitsInfo;
        }

        public Booking GetBooking(int bookingId)
        {
            if (!_bookings.ContainsKey(bookingId))
                return null;
            return _bookings[bookingId];
        }

        public CalendarOutputResource GetCalendar(int rentalId, DateTime start, int nights)
        {
            if (!_rentals.ContainsKey(rentalId))
                return null;

            var rental = _rentals[rentalId];
            var prepTime = rental.PreparationTimeInDays;
            var result = new CalendarOutputResource
            {
                RentalId = rentalId,
                Dates = new List<CalendarDateOutputResource>()
            };
            for (var i = 0; i < nights; i++)
            {
                var date = new CalendarDateOutputResource
                {
                    Date = start.Date.AddDays(i)
                };

                foreach (var unit in rental.UnitsInformation)
                {
                    foreach (var booking in unit.Bookings)
                    {
                        if (booking.Start <= date.Date && booking.Start.AddDays(booking.Nights) > date.Date)
                            date.Bookings.Add(new CalendarBookingOutputResource
                            {
                                Id = booking.Id,
                                Unit = booking.UnitInformation.Number
                            });
                        else if (booking.Start.AddDays(booking.Nights) <= date.Date && booking.Start.AddDays(booking.Nights + prepTime) > date.Date)
                            date.PreparationTimes.Add(new UnitOutputResource() { Unit = booking.UnitInformation.Number });
                    }
                }

                result.Dates.Add(date);
            }

            return result;
        }

        public Rental GetRental(int rentalId)
        {
            if (!_rentals.ContainsKey(rentalId))
                return null;
            return _rentals[rentalId];
        }

        public IdOutputResource PostBooking(BookingInputResource model)
        {
            if (!_rentals.ContainsKey(model.RentalId))
                return new IdOutputResource();

            var rental = _rentals[model.RentalId];
            var prepTime = rental.PreparationTimeInDays;
            foreach (var unit in rental.UnitsInformation)
            {
                bool canBeRented = true;
                foreach (var booking in unit.Bookings)
                {
                    if ((booking.Start <= model.Start.Date && booking.Start.AddDays(booking.Nights + prepTime) > model.Start.Date)
                        || (booking.Start < model.Start.AddDays(model.Nights) && booking.Start.AddDays(booking.Nights + prepTime) >= model.Start.AddDays(model.Nights))
                        || (booking.Start > model.Start && booking.Start.AddDays(booking.Nights + prepTime) < model.Start.AddDays(model.Nights)))
                    {
                        canBeRented = false;
                        break;
                    }
                }
                if (canBeRented)
                {
                    var key = new IdOutputResource { Id = _bookings.Keys.Count + 1 };
                    var booking = new Booking
                    {
                        Id = key.Id,
                        Nights = model.Nights,
                        UnitInformation = unit,
                        Start = model.Start.Date
                    };
                    _bookings.Add(key.Id, booking);
                    unit.Bookings.Add(booking);
                    return key;
                }
            }
            return null;
        }

        public IdOutputResource PostRental(RentalInputResource model)
        {
            var key = new IdOutputResource { Id = _rentals.Keys.Count + 1 };
            var rental = new Rental
            {
                Id = key.Id,
                Units = model.Units,
                PreparationTimeInDays = model.PreparationTimeInDays
            };

            if (model.UnitsDescriptions.Count == model.Units)
            {
                int number = 1;
                foreach (var unitDesc in model.UnitsDescriptions)
                {
                    int id = _unitsInfo.Keys.Count + 1;
                    var unit = new UnitInformation()
                    {
                        Id = id,
                        Description = unitDesc,
                        Number = number++,
                        Rental = rental
                    };
                    _unitsInfo.Add(id, unit);
                    rental.UnitsInformation.Add(unit);
                }
            }
            else
            {
                for (int i = 1; i <= model.Units; i++)
                {
                    int id = _unitsInfo.Keys.Count + 1;
                    var unit = new UnitInformation()
                    {
                        Id = id,
                        Number = i,
                        Rental = rental
                    };

                    _unitsInfo.Add(id, unit);
                    rental.UnitsInformation.Add(unit);
                }
            }

            _rentals.Add(key.Id, rental);
            return key;
        }

        public IdOutputResource PutRental(int rentalId, RentalInputResource model)
        {
            if (!_rentals.ContainsKey(rentalId))
                return null;

            var rental = _rentals[rentalId];

            if (CanPreparationTimeBeModified(rental, model.PreparationTimeInDays))
            {
                if (rental.Units < model.Units)
                {
                    int key = _unitsInfo.Keys.Max() + 1;
                    int number = _unitsInfo.Max(u => u.Value.Number) + 1;

                    for (int i = 0; i < model.Units - rental.Units; i++)
                    {
                        var unit = new UnitInformation()
                        {
                            Id = key++,
                            Number = number++,
                            Rental = rental
                        };
                        rental.UnitsInformation.Add(unit);
                        _unitsInfo.Add(key, unit);
                    }
                }
                else
                {
                    int unitsToDelete = 0;
                    var dateNow = DateTime.Now.Date;
                    foreach (var unit in rental.UnitsInformation)
                    {
                        bool canBeDeleted = true;
                        foreach(var booking in unit.Bookings)
                        {
                            if (booking.Start < dateNow && booking.Start.AddDays(booking.Nights) > dateNow)
                            {
                                canBeDeleted = false;
                                break;
                            } 
                        }
                        if (canBeDeleted) unitsToDelete++;
                    }

                    if (unitsToDelete < rental.Units - model.Units)
                        return null;
                    DeleteUnitsFromRental(rental, rental.Units - model.Units, dateNow);
                }
                rental.PreparationTimeInDays = model.PreparationTimeInDays;
                rental.Units = rental.UnitsInformation.Count;
            }
            else 
                return null;
            return new IdOutputResource() { Id = rentalId };
        }

        private void DeleteUnitsFromRental(Rental rental, int unitsAmount, DateTime date)
        {
            int deletedUnits = 0;
            for (int i = rental.Units - 1; i >= 0; i--)
            {
                bool canBeDeleted = true;
                foreach (var booking in rental.UnitsInformation[i].Bookings)
                {
                    if (booking.Start < date && booking.Start.AddDays(booking.Nights) > date)
                    {
                        canBeDeleted = false;
                        break;
                    }
                }
                if (canBeDeleted)
                {
                    deletedUnits++;
                    _unitsInfo.Remove(rental.UnitsInformation[i].Id);
                    rental.UnitsInformation.Remove(rental.UnitsInformation[i]);
                }
                if (deletedUnits == unitsAmount)
                    return;
            }
        }

        private bool CanPreparationTimeBeModified(Rental rental, int prepTime)
        {
            if (prepTime <= rental.PreparationTimeInDays && prepTime >= 0)
                return true;

            foreach (var unit in rental.UnitsInformation)
            {
                var bookings = unit.Bookings;
                for (int i = 0; i < bookings.Count; i++)
                {
                    var beginBooking = bookings[i].Start;
                    var endBooking = bookings[i].Start.AddDays(bookings[i].Nights + prepTime);
                    for (int j = i + 1; j < bookings.Count; j++)
                    {
                        if (beginBooking <= bookings[j].Start.Date && endBooking > bookings[j].Start.Date)
                            return false;
                    }
                }
            }
            return true;
        }
    }
}
