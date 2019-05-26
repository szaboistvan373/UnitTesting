using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests {
    [TestFixture]
    class FizzBuzzTests {
        [Test]
        public void GetOutPut_DivisibleByBoth3And5_ReturnFizzBuzz([Values(0, 15, 30, 45, 75)] int input) {
            var result = FizzBuzz.GetOutput(input);

            Assert.That(result, Is.EqualTo("FizzBuzz"));
        }

        [Test]
        public void GetOutPut_DivisibleBy3_ReturnFizz([Values(3, 6, 9, 93)] int input) {
            var result = FizzBuzz.GetOutput(input);

            Assert.That(result, Is.EqualTo("Fizz"));
        }

        [Test]
        public void GetOutPut_DivisibleBy5_ReturnBuzz([Values(5, 10, 25, 100)] int input) {
            var result = FizzBuzz.GetOutput(input);

            Assert.That(result, Is.EqualTo("Buzz"));
        }

        [Test]
        [TestCase(3, "Fizz")]
        [TestCase(12, "Fizz")]
        [TestCase(5, "Buzz")]
        [TestCase(10, "Buzz")]
        [TestCase(65, "Buzz")]
        [TestCase(15, "FizzBuzz")]
        [TestCase(30, "FizzBuzz")]
        [TestCase(105, "FizzBuzz")]
        public void GetOutPut_WhenCalled_ReturnProperValue(int input, string output) {
            var result = FizzBuzz.GetOutput(input);

            Assert.That(result,Is.EqualTo(output));
        }
    }
}