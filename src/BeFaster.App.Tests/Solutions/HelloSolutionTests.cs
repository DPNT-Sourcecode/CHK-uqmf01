using BeFaster.App.Solutions;
using NUnit.Framework;

namespace BeFaster.App.Tests.Solutions
{
    [TestFixture]
    public class HelloSolutionTests
    {
        [Test]
        public void TestHello()
        {
            var name = "John";
            Assert.That(HelloSolution.Hello(name), Is.EqualTo("Hello World, I'm John"));
        }
    }
}
