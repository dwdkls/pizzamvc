using System.Collections;

namespace Pizza.Framework.IntegrationTests.Extensions
{
    public static class ReallyHave
    {
        public static SameItemsConstraint SameItemsAs(IList expected)
        {
            return new SameItemsConstraint(expected);
        }
    }
}