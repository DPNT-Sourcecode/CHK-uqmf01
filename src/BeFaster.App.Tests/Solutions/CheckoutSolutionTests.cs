using BeFaster.App.Solutions.Checkout;
using Moq;
using NUnit.Framework;

namespace BeFaster.App.Tests.Solutions
{
    [TestFixture]
    class WhenTryingToBuyAnItemThatDoesNotExist
    {
        [Test]
        public void ThePriceIsMinusOne()
        {
            var priceDatabase = new Mock<IPriceDatabase>();
            priceDatabase.Setup(x => x.GetIndividualPriceFor('X'))
                .Throws(new SkuInvalidException('X'));
            var checkoutPricer = new CheckoutPricer(priceDatabase.Object);
            Assert.That(checkoutPricer.CalculatePrice("X"), Is.EqualTo(-1));
        }
    }

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

    [TestFixture]
    class WhenBuyingTwoDifferentItems
    {
        [Test]
        public void ThePriceIsTheSumOfTheTwoSkuPrices()
        {
            var priceDatabase = new Mock<IPriceDatabase>();
            priceDatabase.Setup(x => x.GetIndividualPriceFor('X'))
                .Returns(10);
            priceDatabase.Setup(x => x.GetIndividualPriceFor('Y'))
                .Returns(5);
            var checkoutPricer = new CheckoutPricer(priceDatabase.Object);
            Assert.That(checkoutPricer.CalculatePrice("XY"), Is.EqualTo(15));
        }
    }

    [TestFixture]
    class WhenTheBasketContainsAnInvalidSku
    {
        [Test]
        public void ThePriceIsMinusOne()
        {
            var priceDatabase = new Mock<IPriceDatabase>();
            priceDatabase.Setup(x => x.GetIndividualPriceFor('X'))
                .Returns(10);
            priceDatabase.Setup(x => x.GetIndividualPriceFor('Y'))
                .Throws(new SkuInvalidException('Y'));
            var checkoutPricer = new CheckoutPricer(priceDatabase.Object);
            Assert.That(checkoutPricer.CalculatePrice("XY"), Is.EqualTo(-1));
        }
    }
}
