using System;
using System.Linq;
using Pizza.Contracts.Persistence.Attributes;

namespace Pizza.Framework.DataGeneration.SpecimenBuilders
{
    internal sealed class UnicodeStringSpecimenBuilder : StringSpecimenBuilderBase
    {
        private static readonly Type[] MyAttributeTypes = new[] { typeof(UnicodeStringAttribute) };

        protected override Type[] AttributeTypes
        {
            get { return MyAttributeTypes; }
        }

        protected override string BuildString(string prefix, int length)
        {
            var value = string.Format("{0} {1} {2}", prefix, Guid.NewGuid(), LoremIpsum); //.Substring(0, Random.Next(LoremIpsum.Length)));
            return new string(value.Take(length).ToArray());
        }
    }
}