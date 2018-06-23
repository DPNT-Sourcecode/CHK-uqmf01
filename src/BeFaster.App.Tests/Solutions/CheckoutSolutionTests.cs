using BeFaster.App.Solutions;
using NUnit.Framework;

namespace BeFaster.App.Tests.Solutions
{
    [TestFixture]
    class CheckoutSolutionTests
    {
        [TestCase("A", 50)]
        [TestCase("B", 30)]
        [TestCase("C", 20)]
        [TestCase("D", 15)]
        [TestCase("AABC", 150)]
        [TestCase("AAABBC", 195)]
        [TestCase("ABCD", 115)]
        [TestCase("DDDB", 75)]
        [TestCase("AAAAAA", 260)]
        [TestCase("AABBCCDD", 215)]
        public void CanBuyTwoAs(string skus, int price)
        {
            Assert.That(CheckoutSolution.Checkout(skus), Is.EqualTo(price));
        }
    }
}
