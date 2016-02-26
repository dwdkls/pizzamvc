using System;
using System.Linq;
using System.Reflection;
using Pizza.Persistence.Attributes;
using Ploeh.AutoFixture.Kernel;

namespace Pizza.Framework.DataGeneration.SpecimenBuilders
{
    internal abstract class StringSpecimenBuilderBase : ISpecimenBuilder
    {
        // TODO: use StringGenerator class
        protected const string LoremIpsum = " Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse id erat blandit, fermentum lorem et, lobortis nisl. Morbi id ante nec ligula adipiscing gravida in vel turpis. Sed aliquam ipsum nec mauris ultrices, vel congue neque convallis. Sed erat turpis, tristique ac nulla nec, laoreet molestie orci. Etiam posuere, mi scelerisque porta tincidunt, magna lacus consequat risus, nec luctus lectus est non felis. Nam tempor, nisl eget fermentum placerat, risus orci sollicitudin est, a pulvinar orci magna et enim. Praesent suscipit felis eget tempor tristique.";

        protected static Random Random = new Random();

        protected abstract Type[] AttributeTypes { get; }

        protected abstract string BuildString(string prefix, int length);

        public object Create(object request, ISpecimenContext context)
        {
            var propertyInfo = request as PropertyInfo;

            if (propertyInfo != null && propertyInfo.PropertyType == typeof(string))
            {
                var attribute = propertyInfo.GetCustomAttributes(true)
                    .SingleOrDefault(x => this.AttributeTypes.Contains(x.GetType()));

                if (attribute != null)
                {
                    var stringAttribute = (StringAttribute)attribute;
                    return this.BuildString(propertyInfo.Name, stringAttribute.Length);
                }
            }

            return new NoSpecimen(request);
        }
    }
}