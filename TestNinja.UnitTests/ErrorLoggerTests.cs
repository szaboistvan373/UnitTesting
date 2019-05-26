using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests {
    [TestFixture()]
    class ErrorLoggerTests {
        private ErrorLogger _errorLogger;

        [SetUp]
        public void SetUp() {
            _errorLogger = new ErrorLogger();
        }

        [Test]
        public void Log_InputContainsValue_SetLastErrorProperty() {
            _errorLogger.Log("a");
            Assert.That(_errorLogger.LastError, Is.EqualTo("a"));
        }

        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void Log_InputIsEmpty_ThrowArgumentNullException(string error) {
            Assert.That(() => _errorLogger.Log(error), Throws.ArgumentNullException);
        }

        [Test]
        public void Log_ValidError_RaiseErrorLoggedEvent() {
            var id = Guid.Empty;

            _errorLogger.ErrorLogged += (sender, args) => { id = args; };

            _errorLogger.Log("a");

            Assert.That(id, Is.Not.EqualTo(Guid.Empty));
        }

        [Test]
        public void Log_InputIsNullOrWhiteSpace_ThrowArgumentNullEaaxception([Values("", " ", null)] string input) {
            Assert.That(() => _errorLogger.Log(input), Throws.ArgumentNullException);
        }
    }
}