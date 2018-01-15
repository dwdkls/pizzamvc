using System;
using System.Linq;
using Pizza.Persistence.Attributes;

namespace Pizza.Framework.DataGeneration.SpecimenBuilders
{
    internal sealed class UnicodeStringSpecimenBuilder : StringSpecimenBuilderBase
    {
        protected override Type[] AttributeTypes => new[] { typeof(UnicodeStringAttribute) };

        protected override string BuildString(string prefix, int length)
        {
            var value = $"{prefix} {Guid.NewGuid()} {LoremIpsum}"; //.Substring(0, Random.Next(LoremIpsum.Length)));
            return new string(value.Take(length).ToArray());
        }
    }
}