using BeFaster.App.Solutions;
using NUnit.Framework;

namespace BeFaster.App.Tests.Solutions
{
    [TestFixture]
    public class SumSolutionTests
    {
        [TestCase(1, 1, ExpectedResult = 2)]
        [TestCase(0, 0, ExpectedResult = 0)]
        [TestCase(100, 100, ExpectedResult = 200)]
        [TestCase(100, 0, ExpectedResult = 100)]
        public int ComputeSum(int x, int y)
        {
            return SumSolution.Sum(x, y);
        }
    }
}
