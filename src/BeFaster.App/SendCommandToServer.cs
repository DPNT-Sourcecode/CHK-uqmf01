using System;
using BeFaster.App.Solutions;
using BeFaster.Runner;
using BeFaster.Runner.Extensions;
using BeFaster.Runner.Utils;
using TDL.Client;
using TDL.Client.Runner;

namespace BeFaster.App
{
    internal class SendCommandToServer
    {
        /// <summary>
        /// ~~~~~~~~~~ Running the system: ~~~~~~~~~~~~~
        ///
        ///   From IDE:
        ///      Configure the "BeFaster.App" solution to Run on External Console then run.
        ///
        ///   From command line:
        ///      msbuild befaster.sln; src\BeFaster.App\bin\Debug\BeFaster.App.exe
        ///
        ///   To run your unit tests locally:
        ///      Run the "BeFaster.App.Tests - Unit Tests" configuration.
        ///
        /// ~~~~~~~~~~ The workflow ~~~~~~~~~~~~~
        ///
        ///   By running this file you interact with a challenge server.
        ///   The interaction follows a request-response pattern:
        ///        * You are presented with your current progress and a list of actions.
        ///        * You trigger one of the actions by typing it on the console.
        ///        * After the action feedback is presented, the execution will stop.
        ///
        ///   +------+-------------------------------------------------------------+
        ///   | Step | The usual workflow                                          |
        ///   +------+-------------------------------------------------------------+
        ///   |  1.  | Run this file.                                              |
        ///   |  2.  | Start a challenge by typing "start".                        |
        ///   |  3.  | Read description from the "challenges" folder               |
        ///   |  4.  | Implement the required method in                            |
        ///   |      |   .\src\BeFaster.App\Solutions                              |
        ///   |  5.  | Deploy to production by typing "deploy".                    |
        ///   |  6.  | Observe output, check for failed requests.                  |
        ///   |  7.  | If passed, go to step 3.                                    |
        ///   +------+-------------------------------------------------------------+
        ///
        ///   You are encouraged to change this project as you please:
        ///        * You can use your preferred libraries.
        ///        * You can use your own test framework.
        ///        * You can change the file structure.
        ///        * Anything really, provided that this file stays runnable.
        ///
        /// </summary>
        /// <param name="args">Action.</param>
        private static void Main(string[] args)
        {
            var runner = new QueueBasedImplementationRunner.Builder()
                .SetConfig(Utils.GetRunnerConfig())
                .WithSolutionFor("sum", p => SumSolution.Sum(p[0].AsInt(), p[1].AsInt()))
                .WithSolutionFor("hello", p => HelloSolution.Hello(p[0]))
                .WithSolutionFor("fizz_buzz", p => FizzBuzzSolution.FizzBuzz(p[0].AsInt()))
                .WithSolutionFor("checkout", p => CheckoutSolution.Checkout(p[0]))
                .Create();

            ChallengeSession.ForRunner(runner)
                .WithConfig(Utils.GetConfig())
                .WithActionProvider(new UserInputAction(args))
                .Start();

            Console.Write("Press any key to continue . . . ");
            Console.ReadKey();
        }
    }
}
