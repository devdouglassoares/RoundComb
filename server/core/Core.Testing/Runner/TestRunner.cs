using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Core;

namespace Core.Testing.Runner
{
    public class TestRunner
    {
        /// <summary>
        /// This method will load all nunit tests in specified assembly, run them and return detailed result
        /// 
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns>Detailed result. items property will contain details about all runned tests grouped by test classes</returns>
        public IntegrationTestsResults Run(Assembly assembly)
        {
            CoreExtensions.Host.InitializeService();

            TestPackage testPackage = new TestPackage(new System.Uri(assembly.CodeBase).LocalPath);
            SimpleTestRunner testRunner = new SimpleTestRunner();
            testRunner.Load(testPackage);

            TestResult testResult = testRunner.Run(new NullListener(), TestFilter.Empty, false, LoggingThreshold.All);

            var formatted = this.FormatResults(testResult, new List<TestResult> {});
            return new IntegrationTestsResults
                {
                    status = (IntegrationTestResultState)testResult.ResultState,
                    time = testResult.Time,
                    items = formatted,
                    passed =
                        formatted.SelectMany(t => t.Results.Cast<TestResult>())
                            .Count(r => r.ResultState == ResultState.Success || r.ResultState == ResultState.Ignored),
                    total = formatted.SelectMany(t => t.Results.Cast<TestResult>()).Count(),
                    isSuccess = testResult.ResultState == ResultState.Success
                };
        }

        private List<TestResult> FormatResults(TestResult result, List<TestResult> list)
        {
            if (result.Test.TestType == "TestFixture")
            {
                list.Add(result);
                return list;
            }

            if (result.HasResults)
            {

                foreach (TestResult r in result.Results)
                {
                    this.FormatResults(r, list);
                }
            }

            return list;
        }
    }
}
