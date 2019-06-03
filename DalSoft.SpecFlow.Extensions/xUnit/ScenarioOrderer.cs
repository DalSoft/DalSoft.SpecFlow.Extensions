using System;
using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace DalSoft.SpecFlow.Extensions.xUnit
{
    public class ScenarioOrderer : ITestCaseOrderer
    {
        public const string TypeName = "DalSoft.SpecFlow.Extensions.xUnit.ScenarioOrderer";
        public const string AssemblyName = "DalSoft.SpecFlow.Extensions";

        public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases) where TTestCase : ITestCase
        {
            var sortedList = new SortedList<int, TTestCase>();
            var unOrdered = 100000;

            foreach (var testCase in testCases)
            {
                int order;
                if (testCase.Traits.SelectMany(_ => _.Value).Any(_ => _.StartsWith("ScenarioOrder")))
                    order = Convert.ToInt32(testCase.Traits.SelectMany(_ => _.Value).Single(_ => _.StartsWith("ScenarioOrder")).Replace("ScenarioOrder", string.Empty));
                else
                {
                    unOrdered++;
                    order = unOrdered;
                }

                sortedList.Add(order, testCase);
            }

            return sortedList.Values;
        }
    }
}
