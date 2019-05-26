using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests {
    [TestFixture()]
    class CustomerControllerTests {
        private CustomerController _customerController;

        [SetUp]
        public void SetUp() {
            _customerController = new CustomerController();
        }

        [Test]
        public void GetCustomer_IdIsZero_ReturnNotFound() {
            var result = _customerController.GetCustomer(0);

            Assert.That(result, Is.TypeOf<NotFound>());
        }

        [Test]
        public void GetCustomer_IdIsNotZero_ReturnOk() {
            var result = _customerController.GetCustomer(2);

            Assert.That(result, Is.TypeOf<Ok>());
        }
    }
}
