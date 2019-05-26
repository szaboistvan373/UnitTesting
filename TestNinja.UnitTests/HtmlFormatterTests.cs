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
    class HtmlFormatterTests {
        [Test]
        public void FormatAsBold_WhenCalled_EncloseTheStringWithStrongElement() {
            var htmlFormatter = new HtmlFormatter();

            var result = htmlFormatter.FormatAsBold("myString");

            // Specific
            Assert.That(result, Is.EqualTo("<strong>myString</strong>").IgnoreCase);
        }
    }
}
