using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KellermanSoftware.CompareNetObjects;
using NUnit.Asserts.Compare;
using NUnit.Framework.Constraints;

namespace Pizza.Framework.IntegrationTests.Extensions
{
    public class SameItemsConstraint : CompareConstraint
    {
        private readonly List<ComparisonResult> comparisonResults = new List<ComparisonResult>();
        private readonly IList expectedList;

        public SameItemsConstraint(IList expected)
        {
            this.expectedList = expected;
        }

        public override bool Matches(object actual)
        {
            this.actual = actual;

            var actualList = (IList)actual;

            for (int i = 0; i < this.expectedList.Count; i++)
            {
                var comparisonResult = this._compareLogic.Compare(this.expectedList[i], actualList[i]);
                if (!comparisonResult.AreEqual)
                {
                    this.comparisonResults.Add(comparisonResult);
                }
            }

            return !this.comparisonResults.Any();
        }

        public override void WriteDescriptionTo(MessageWriter writer)
        {
            if (!this.comparisonResults.Any())
                return;

            var sb = new StringBuilder();
            foreach (var result in this.comparisonResults)
            {
                var difference = result.Differences.Single();

                sb.AppendFormat("{0} between: {1} and: {2}\n",
                    result.DifferencesString, difference.ParentObject1.Target, difference.ParentObject2.Target);
            }

            writer.Write("Items in collection are not same. Please inspect differences:\n\n{0}", sb);
        }
    }
}