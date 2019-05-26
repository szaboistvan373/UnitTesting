using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking {
    [TestFixture]
    class OrderServiceTests {
        [Test]
        public void PlaceOrder_WhenCalled_StoreOrder() {
            var storage = new Mock<IStorage>();
            var orderService = new OrderService(storage.Object);

            var newOrder = new Order();

            orderService.PlaceOrder(newOrder);

            storage.Verify(s => s.Store(newOrder));
        }
    }
}
