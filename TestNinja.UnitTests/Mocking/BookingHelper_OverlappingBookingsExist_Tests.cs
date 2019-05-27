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
    class BookingHelper_OverlappingBookingsExist_Tests {
        private Mock<IBookingRepository> _bookRepository;

        [SetUp]
        public void SetUp() {
            _bookRepository = new Mock<IBookingRepository>();
        }

        [Test]
        public void StatusIsCancelled_ReturnEmptyString() {
            var booking = new Booking {
                Status = "Cancelled"
            };

            var result = BookingHelper.OverlappingBookingsExist(_bookRepository.Object, booking);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void StatusIsOkAndNoOverlappingBooking_ReturnEmptyString() {
            var booking = new Booking {
                Status = "Ok"
            };

            _bookRepository.Setup(br => br.GetOverlappingBooking(It.IsAny<Booking>())).Returns(default(Booking));

            var result = BookingHelper.OverlappingBookingsExist(_bookRepository.Object, booking);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void StatusIsOkAndOverlappingBookingExists_ReturnStringHonk() {
            var booking = new Booking {
                Status = "Ok"
            };

            var overlappingBooking = new Booking {
                Reference = "xy123"
            };

            _bookRepository.Setup(br => br.GetOverlappingBooking(It.IsAny<Booking>())).Returns(overlappingBooking);

            var result = BookingHelper.OverlappingBookingsExist(_bookRepository.Object, booking);

            Assert.That(result, Is.EqualTo("xy123"));
        }
    }
}