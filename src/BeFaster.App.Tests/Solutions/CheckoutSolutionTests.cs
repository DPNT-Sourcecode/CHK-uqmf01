using BeFaster.App.Solutions.Checkout;
using NUnit.Framework;

namespace BeFaster.App.Tests.Solutions
{
    [TestFixture]
    class WhenBuyingASingleItem
    {
        [Test]
        public void ItHasTheCorrectPrice()
        {
            var checkoutPricer = new CheckoutPricer();
            Assert.That(checkoutPricer.CalculatePrice("X"), Is.EqualTo(10));
        }
    }
}