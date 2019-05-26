using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking {
    [TestFixture]
    class ProductTests {
        [Test]
        public void GetPrice_GoldCustomer_Return30PercentDiscounterPrice() {
            var product = new Product {
                ListPrice = 10
            };

            var result = product.GetPrice(new Customer { IsGold = true });

            Assert.That(result, Is.EqualTo(7));
        }

        [Test]
        public void GetPrice_NormalCustomer_ReturnOriginalPrice() {
            var product = new Product {
                ListPrice = 10
            };

            var result = product.GetPrice(new Customer { IsGold = false });

            Assert.That(result, Is.EqualTo(10));
        }
    }
}