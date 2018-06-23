using BeFaster.App.Solutions;
using NUnit.Framework;

namespace BeFaster.App.Tests.Solutions
{
    [TestFixture]
    class CheckoutSolutionTests
    {
        [TestCase("", 0)]
        [TestCase("A", 50)]
        [TestCase("B", 30)]
        [TestCase("C", 20)]
        [TestCase("D", 15)]
        [TestCase("AABC", 150)]
        [TestCase("AAABBC", 195)]
        [TestCase("ABCD", 115)]
        [TestCase("DDDB", 75)]
        [TestCase("AAAAAA", 250)]
        [TestCase("AABBCCDD", 215)]
        [TestCase("AGFJ ", -1)]
        [TestCase("A% ", -1)]
        [TestCase("% ", -1)]
        [TestCase("E", 40)]
        [TestCase("EE", 80)]
        [TestCase("EEB", 80)]
        [TestCase("EEA", 130)]
        [TestCase("EEBB", 110)]
        [TestCase("EEBBB", 125)]
        [TestCase("F", 10)]
        [TestCase("FF", 20)]
        [TestCase("FFF", 20)]
        [TestCase("FFFF", 30)]
        [TestCase("FFFFF", 40)]
        [TestCase("FFFFFF", 40)]
        public void PriceIsCorrectlyCalculated(string skus, int price)
        {
            Assert.That(CheckoutSolution.Checkout(skus), Is.EqualTo(price));
        }
    }
}
