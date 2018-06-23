using BeFaster.App.Solutions.Checkout;
using Moq;
using NUnit.Framework;

namespace BeFaster.App.Tests.Solutions
{
    [TestFixture]
    class WhenBuyingASingleItem
    {
        [Test]
        public void ItHasTheCorrectPrice()
        {
            var priceDatabase = new Mock<IPriceDatabase>();
            priceDatabase.Setup(x => x.GetIndividualPriceFor('X'))
                .Returns(10);
            var checkoutPricer = new CheckoutPricer(priceDatabase.Object);
            Assert.That(checkoutPricer.CalculatePrice("X"), Is.EqualTo(10));
        }
    }
}
