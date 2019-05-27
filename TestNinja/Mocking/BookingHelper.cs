using System;
using System.Collections.Generic;
using System.Linq;

namespace TestNinja.Mocking {
    public interface IBookingRepository {
        IQueryable<Booking> GetActiveBookings(int? excludedBookingId = null);
    }

    public class BookingRepository : IBookingRepository {
        public IQueryable<Booking> GetActiveBookings(int? excludedBookingId = null) {
            var _unitOfWork = new UnitOfWork();

            var bookings =
                _unitOfWork.Query<Booking>()
                    .Where(
                        b => b.Status != "Cancelled");

            if (excludedBookingId.HasValue)
                bookings = bookings.Where(b => b.Id != excludedBookingId.Value);

            return bookings;
        }
    }

    public static class BookingHelper {
        public static string OverlappingBookingsExist(IBookingRepository bookingRepository, Booking booking) {
            if (booking.Status == "Cancelled")
                return string.Empty;

            var activeBookings = bookingRepository.GetActiveBookings(booking.Id);

            var overlappingBooking =
                activeBookings.FirstOrDefault(
                    b =>
                        booking.ArrivalDate < b.DepartureDate && booking.DepartureDate > b.ArrivalDate);

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