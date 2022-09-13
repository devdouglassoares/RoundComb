using System.Collections.Generic;
using NUnit.Core;

namespace Core.Testing.Runner
{
    public class IntegrationTestsResults
    {
        public IntegrationTestResultState status { get; set; }
        public double time{ get; set; }
        public List<TestResult> items { get; set; }
        public int passed { get; set; }
        public int total { get; set; }
        public bool isSuccess { get; set; }
    }
}