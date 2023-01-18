using System;
using VacationRental.Api.Model;
using VacationRental.Api.Resources;

namespace VacationRental.Api.IRepositories
{
    public interface IRentalsRepository
    {
        Rental GetRental(int rentalId);

        IdOutputResource PostRental(RentalInputResource model);

        IdOutputResource PutRental(int rentalId, RentalInputResource model);

        Booking GetBooking(int bookingId);
        IdOutputResource PostBooking(BookingInputResource model);

        CalendarOutputResource GetCalendar(int rentalId, DateTime start, int nights);
    }
}
