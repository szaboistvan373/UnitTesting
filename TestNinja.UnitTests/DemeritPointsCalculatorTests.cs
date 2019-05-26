using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja {
    class DemeritPointsCalculatorTests {
        private DemeritPointsCalculator _calculator;

        [SetUp]
        public void SetUp() {
            _calculator = new DemeritPointsCalculator();
        }

        [Test]
        public void CalculateDemeritPoints_LessThanOrEqualToLimit_ReturnZero([Values(10, 15, 30, 35, 60, 65)]int speed) {
            var result = _calculator.CalculateDemeritPoints(speed);

            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void CalculateDemeritPoints_LessThan0OrMoreThanMaxSpeed_ThrowArgumentOutOfRangeException([Values(-10, -3, 305, 600)]int speed) {
            Assert.That(() => _calculator.CalculateDemeritPoints(speed), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        [TestCase(66, 0)]
        [TestCase(70, 1)]
        [TestCase(71, 1)]
        [TestCase(100, 7)]
        [TestCase(103, 7)]
        public void CalculateDemeritPoints_GreaterThanSpeedLimit_ReturnDemeritPointsBasedOnKmPerDemerit(int speed, int demeritPoints) {
            var result = _calculator.CalculateDemeritPoints(speed);

            Assert.That(result, Is.EqualTo(demeritPoints));
        }
    }
}
