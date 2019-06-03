using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace DalSoft.SpecFlow.Extensions.xUnit
{
    public class FeatureOrderer : ITestCollectionOrderer
    {
        public const string TypeName = "DalSoft.SpecFlow.Extensions.xUnit.FeatureOrderer";
        public const string AssemblyName = "DalSoft.SpecFlow.Extensions";

        public IEnumerable<ITestCollection> OrderTestCollections(IEnumerable<ITestCollection> testCollections)
        {
            var sortedList = new SortedList<int, ITestCollection>();
            var unOrdered = 100000;

            foreach (var testCollection in testCollections)
            {
                var featureClass = testCollection.TestAssembly
                    .Assembly.GetType(testCollection.DisplayName.Replace("Test collection for ", string.Empty))
                    .ToRuntimeType();

                var traits = TraitHelper.GetTraits(featureClass);

                int order;

                if (traits.Any(_ => _.Value.StartsWith("FeatureOrder")))
                    order = Convert.ToInt32(traits.Single(_ => _.Value.StartsWith("FeatureOrder")).Value.Replace("FeatureOrder", string.Empty));
                else
                {
                    unOrdered++;
                    order = unOrdered;
                }

                sortedList.Add(order, testCollection);
            }

            return sortedList.Values;
        }
    }
}