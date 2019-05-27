using System;
using System.Collections.Generic;
using System.Linq;

namespace TestNinja.Mocking {
    public interface IBookingRepository {
        Booking GetOverlappingBooking(Booking booking);
    }

    public class BookingRepository : IBookingRepository {
        private UnitOfWork _unitOfWork;

        public BookingRepository() {
            _unitOfWork = new UnitOfWork();
        }

        public Booking GetOverlappingBooking(Booking booking) {
            var bookings =
                _unitOfWork.Query<Booking>()
                    .Where(
                        b => b.Id != booking.Id && b.Status != "Cancelled");

            var overlappingBooking =
                bookings.FirstOrDefault(
                    b =>
                        booking.ArrivalDate >= b.ArrivalDate
                        && booking.ArrivalDate < b.DepartureDate
                        || booking.DepartureDate > b.ArrivalDate
                        && booking.DepartureDate <= b.DepartureDate);

            return overlappingBooking;
        }
    }

    public static class BookingHelper {
        public static string OverlappingBookingsExist(IBookingRepository bookingRepository, Booking booking) {
            if (booking.Status == "Cancelled")
                return string.Empty;

            var overlappingBooking = bookingRepository.GetOverlappingBooking(booking);

            return overlappingBooking == null ? string.Empty : overlappingBooking.Reference;
        }
    }

    public class UnitOfWork {
        public IQueryable<T> Query<T>() {
            return new List<T>().AsQueryable();
        }
    }

    public class Booking {
        public string Status { get; set; }
        public int Id { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public string Reference { get; set; }
    }
}