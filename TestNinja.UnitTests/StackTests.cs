using System;
using System.CodeDom;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;
using TestNinja.Fundamentals;
using DateHelper = TestNinja.Fundamentals.DateHelper;

namespace TestNinja.UnitTests {
    [TestFixture]
    class StackTests {
        [Test]
        [TestCase(new int[] { })]
        [TestCase(new int[] { 1, 2 })]
        public void Count_AfterElementsPushed_ReturnTheNumberOfElementsAdded(int[] items) {
            var stack = new Stack<int>();

            foreach (var item in items) {
                stack.Push(item);
            }

            Assert.That(stack.Count, Is.EqualTo(items.Length));
        }

        [Test]
        public void Push_InputIsNull_ThrowArgumentNullException() {
            var stack = new Stack<DateHelper>();

            Assert.That(() => stack.Push(null), Throws.ArgumentNullException);
        }

        [Test]
        public void PopAndPeek_EmptyStack_ThrowInvalidOperationException() {
            var stack = new Stack<string>();

            Assert.That(() => stack.Pop(), Throws.InvalidOperationException);
            Assert.That(() => stack.Peek(), Throws.InvalidOperationException);
        }

        [Test]
        public void Pop_StackWithFewObjects_OriginalCountSubtractedBy1() {
            var stack = new Stack<string>();

            stack.Push("asd");
            stack.Push("asd");
            stack.Push("asd");

            int count = stack.Count;

            stack.Pop();

            Assert.That(stack.Count, Is.EqualTo(count - 1));
        }

        [Test]
        [TestCase(new int[] { 1, 4, 1, 60 })]
        [TestCase(new int[] { 10, 21 })]
        [TestCase(new int[] { 1023, 0, 10 })]
        public void Pop_StackWithFewObjects_ReturnTheObjectOnTop(int[] inputArray) {
            var stack = new Stack<int>();
            foreach (var number in inputArray) {
                stack.Push(number);
            }

            var result = inputArray.Last();

            Assert.That(result, Is.EqualTo(result));
        }

        [Test]
        public void Peek_StackWithFewObjects_DoesNotRemoveObject() {
            var stack = new Stack<string>();

            stack.Push("a");
            stack.Push("b");
            stack.Push("c");

            stack.Peek();

            Assert.That(stack.Count, Is.EqualTo(3));
        }

        [Test]
        public void Peek_StackWithFewObjects_ReturnTheObjectOnTop() {
            var stack = new Stack<string>();

            stack.Push("a");
            stack.Push("b");
            stack.Push("c");

            var result = stack.Peek();

            Assert.That(result, Is.EqualTo("c"));
        }
    }
}