using BeFaster.App.Solutions;
using NUnit.Framework;

namespace BeFaster.App.Tests.Solutions
{
    [TestFixture]
    public class WhenSayingHello
    {
        [Test]
        public void TheCorrectGreetingIsGiven()
        {
            Assert.That(HelloSolution.Hello(), Is.EqualTo("Hello, World!"));
        }
    }
}
