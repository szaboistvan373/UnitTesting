using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking {
    [TestFixture]
    class BookingHelper_OverlappingBookingsExistTests {
        private Booking _existingBooking;
        private Mock<IBookingRepository> _bookRepository;

        [SetUp]
        public void SetUp() {
            _bookRepository = new Mock<IBookingRepository>();
            _existingBooking = new Booking {
                ArrivalDate = ArriveOn(2019, 05, 28),
                DepartureDate = DepartOn(2019, 06, 03),
                Reference = "xyz1"
            };

            _bookRepository.Setup(br => br.GetActiveBookings(1)).Returns(new List<Booking> {
                _existingBooking
            }.AsQueryable);
        }

        [Test]
        public void StatusIsCancelled_ReturnEmptyString() {
            var result = BookingHelper.OverlappingBookingsExist(_bookRepository.Object, new Booking {
                Id = 1,
                ArrivalDate = After(_existingBooking.ArrivalDate, days: 1),
                DepartureDate = After(_existingBooking.ArrivalDate, days: 10),
                Status = "Cancelled"
            });

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void BookingStartsAndFinishesBeforeExistingBooking_ReturnEmptyString() {
            var result = BookingHelper.OverlappingBookingsExist(_bookRepository.Object, new Booking {
                ArrivalDate = Before(_existingBooking.ArrivalDate, days: 5),
                DepartureDate = Before(_existingBooking.ArrivalDate, days: 1)
            });

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void BookingStartsBeforeAndFinishesInMiddleOfExistingBooking_ReturnExistingBookingsRefrence() {
            var result = BookingHelper.OverlappingBookingsExist(_bookRepository.Object, new Booking {
                Id = 1,
                ArrivalDate = Before(_existingBooking.ArrivalDate),
                DepartureDate = After(_existingBooking.ArrivalDate),
            });

            Assert.That(result, Is.EqualTo(_existingBooking.Reference));
        }

        [Test]
        public void BookingStartsBeforeAndFinishesAfterExistingBooking_ReturnExistingBookingsRefrence() {
            var result = BookingHelper.OverlappingBookingsExist(_bookRepository.Object, new Booking {
                Id = 1,
                ArrivalDate = Before(_existingBooking.ArrivalDate),
                DepartureDate = After(_existingBooking.ArrivalDate, days: 10),
            });

            Assert.That(result, Is.EqualTo(_existingBooking.Reference));
        }

        [Test]
        public void BookingStartsInTheMiddleOfExistingBookingAndFinisherAfter_ReturnExistingBookingsRefrence() {
            var result = BookingHelper.OverlappingBookingsExist(_bookRepository.Object, new Booking {
                Id = 1,
                ArrivalDate = After(_existingBooking.ArrivalDate, days: 1),
                DepartureDate = After(_existingBooking.ArrivalDate, days: 10),
            });

            Assert.That(result, Is.EqualTo(_existingBooking.Reference));
        }

        [Test]
        public void BookingStartsAndFinishesInTheMiddleOfExistingBooking_ReturnExistingBookingsRefrence() {
            var result = BookingHelper.OverlappingBookingsExist(_bookRepository.Object, new Booking {
                Id = 1,
                ArrivalDate = After(_existingBooking.ArrivalDate, days: 1),
                DepartureDate = Before(_existingBooking.DepartureDate, days: 1),
            });

            Assert.That(result, Is.EqualTo(_existingBooking.Reference));
        }

        [Test]
        public void BookingStartsAndFinishesAfterAnExistingBooking_ReturnEmptyString() {
            var result = BookingHelper.OverlappingBookingsExist(_bookRepository.Object, new Booking {
                Id = 1,
                ArrivalDate = After(_existingBooking.DepartureDate, days: 1),
                DepartureDate = After(_existingBooking.DepartureDate, days: 5),
            });

            Assert.That(result, Is.Empty);
        }

        private DateTime Before(DateTime dateTime, int days = 1) {
            return dateTime.AddDays(-days);
        }

        private DateTime After(DateTime dateTime, int days = 1) {
            return dateTime.AddDays(days);
        }

        private DateTime ArriveOn(int year, int month, int day) {
            return new DateTime(year, month, day, 14, 0, 0);
        }

        private DateTime DepartOn(int year, int month, int day) {
            return new DateTime(year, month, day, 10, 0, 0);
        }
    }
}