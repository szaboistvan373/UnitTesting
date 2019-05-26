using Moq;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking {
    [TestFixture]
    class EmployeeControllerTests {
        private EmployeeController _employeeController;
        private Mock<IEmployeeStorage> _storage;

        [SetUp]
        public void SetUp() {
            _storage = new Mock<IEmployeeStorage>();
            _employeeController = new EmployeeController {
                EmployeeStorage = _storage.Object
            };
        }

        [Test]
        public void DeleteEmployee_WhenCalled_DeleteThenEmployeeFromDb() {
            _employeeController.DeleteEmployee(10);

            _storage.Verify(s => s.DeleteEmployee(10));
        }

        [Test]
        public void DeleteEmployee_WhenCalled_ReturnRedirectToEmployeesAction() {
            var result = _employeeController.DeleteEmployee(10);

            Assert.That(result, Is.TypeOf<RedirectResult>());
        }
    }
}