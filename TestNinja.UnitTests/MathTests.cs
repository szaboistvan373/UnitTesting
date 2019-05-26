using System;
using System.Linq;
using System.Runtime.InteropServices;
using NUnit.Framework;
using Math = TestNinja.Fundamentals.Math;

namespace TestNinja.UnitTests {
    [TestFixture]
    public class MathTests {
        private Math _math;

        [SetUp]
        public void SetUp() {
            _math = new Math();
        }

        [Test]
        public void Add_WhenCalled_ReturnSumOfArguments() {
            var result = _math.Add(1, 2);

            Assert.That(result, Is.EqualTo(3));
        }

        [Test]
        [TestCase(2, 1, 2)]
        [TestCase(1, 2, 2)]
        [TestCase(2, 2, 2)]
        public void Max_WhenCalled_ReturnTheGreaterArgument(int a, int b, int expectedResult) {
            var result = _math.Max(a, b);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetOddNumbers_LimitIsGreaterThanZero_ReturnOddNumbersUUpToLimit() {
            var result = _math.GetOddNumbers(9);

            Assert.That(result, Is.EquivalentTo(new[] { 1, 3, 5, 7, 9 }));
        }

        [Test]
        public void GetOddNumbers_LimitIsLessThanZero_ReturnOddNumbersUUpToLimit() {
            var result = _math.GetOddNumbers(-1);
            Assert.That(result, Is.Empty);
        }
    }
}