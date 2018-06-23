using BeFaster.App.Solutions;
using NUnit.Framework;

namespace BeFaster.App.Tests.Solutions
{
    [TestFixture]
    public class WhenSayingHello
    {
        [TestCase("John")]
        [TestCase("Peter")]
        [TestCase("Person_With*Symbols-In&There)Name Lastname")]
        public void TheCorrectGreetingIsGivenWithTheFriendsName(string friendName)
        {
            Assert.That(HelloSolution.Hello(friendName), Is.EqualTo($"Hello, {friendName}!"));
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase(" ")]
        public void WeGreetTheWorldIfThereIsNoName(string friendName)
        {
            Assert.That(HelloSolution.Hello(friendName), Is.EqualTo($"Hello, World!"));
        }
    }
}
