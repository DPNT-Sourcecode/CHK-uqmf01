using BeFaster.App.Solutions.Checkout;
using Moq;
using NUnit.Framework;

namespace BeFaster.App.Tests.Solutions
{
    // todo This file needs a bit of a refacter! 
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
    class WhenTheBasketContainsAnInvalidSkuAsWellAsValidSkus
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

    [TestFixture]
    class WhenAThreeMultiPriceOfferExists
    {
        private Mock<IPriceDatabase> priceDatabase;

        [SetUp]
        public void SetUp()
        {
            priceDatabase = new Mock<IPriceDatabase>();
            priceDatabase.Setup(x => x.GetIndividualPriceFor('X'))
                .Returns(10);
            priceDatabase.Setup(x => x.GetMultiPriceOfferFor('X'))
                .Returns(new MultiPrice[] { new MultiPrice(quantity: 3, price: 25) });
        }

        [TestCase("X", 10)]
        [TestCase("XX", 20)]
        [TestCase("XXX", 25)]
        [TestCase("XXXX", 35)]
        [TestCase("XXXXX", 45)]
        [TestCase("XXXXXX", 50)]
        [TestCase("XXXXXXX", 60)]
        public void BuyingSeveralItemsHasTheCorrectPrice(string skus, int correctPrice)
        {
            var checkoutPricer = new CheckoutPricer(priceDatabase.Object);
            Assert.That(checkoutPricer.CalculatePrice(skus), Is.EqualTo(correctPrice));
        }
    }

    [TestFixture]
    class WhenABadMultiPriceOfferExists
    {
        private Mock<IPriceDatabase> priceDatabase;

        [SetUp]
        public void SetUp()
        {
            priceDatabase = new Mock<IPriceDatabase>();
            priceDatabase.Setup(x => x.GetIndividualPriceFor('X'))
                .Returns(10);
            priceDatabase.Setup(x => x.GetMultiPriceOfferFor('X'))
                .Returns(new MultiPrice[] { new MultiPrice(quantity: 2, price: 25) });
        }

        [TestCase("X", 10)]
        [TestCase("XX", 20)]
        [TestCase("XXX", 30)]
        [TestCase("XXXX", 40)]
        [TestCase("XXXXX", 50)]
        public void TheCustomerIsNotRippedOff(string skus, int correctPrice)
        {
            var checkoutPricer = new CheckoutPricer(priceDatabase.Object);
            Assert.That(checkoutPricer.CalculatePrice(skus), Is.EqualTo(correctPrice));
        }
    }

    [TestFixture]
    class WhenAGetOneFreeOfferExists
    {
        private Mock<IPriceDatabase> priceDatabase;

        [SetUp]
        public void SetUp()
        {
            priceDatabase = new Mock<IPriceDatabase>();
            priceDatabase.Setup(x => x.GetIndividualPriceFor('X'))
                .Returns(10);
            priceDatabase.Setup(x => x.GetIndividualPriceFor('Y'))
                .Returns(5);
            priceDatabase.Setup(x => x.GetGetOneFreeOfferFor('X'))
                .Returns(new GetOneFreeOffer(quantity: 2, freeSku: 'Y'));
        }

        [TestCase("XXY", 20)]
        [TestCase("XX", 20)]
        [TestCase("XXYY", 25)]
        [TestCase("XXXXYY", 40)]
        public void BuyingSeveralItemsHasTheCorrectPrice(string skus, int correctPrice)
        {
            var checkoutPricer = new CheckoutPricer(priceDatabase.Object);
            Assert.That(checkoutPricer.CalculatePrice(skus), Is.EqualTo(correctPrice));
        }
    }

    [TestFixture]
    class WhenTwoMultiPriceOffersExistsForTheSameSku
    {
        private Mock<IPriceDatabase> priceDatabase;

        [SetUp]
        public void SetUp()
        {
            priceDatabase = new Mock<IPriceDatabase>();
            priceDatabase.Setup(x => x.GetIndividualPriceFor('X'))
                .Returns(10);
            priceDatabase.Setup(x => x.GetMultiPriceOfferFor('X'))
                .Returns(new MultiPrice[] {
                    new MultiPrice(quantity: 2, price: 15),
                    new MultiPrice(quantity: 3, price: 22)});
        }

        [TestCase("X", 10)]
        [TestCase("XX", 15)]
        [TestCase("XXX", 22)]
        [TestCase("XXXX", 30, Ignore = "This is a bad offer which shouldn't be allowed in prod")]// 2+2
        [TestCase("XXXXX", 37)]//2+3
        [TestCase("XXXXXX", 44)]//3+3
        [TestCase("XXXXXXX", 54)]//3+3+1
        public void BuyingSeveralItemsHasTheCorrectPrice(string skus, int correctPrice)
        {
            var checkoutPricer = new CheckoutPricer(priceDatabase.Object);
            Assert.That(checkoutPricer.CalculatePrice(skus), Is.EqualTo(correctPrice));
        }
    }
}
