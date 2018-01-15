using System;
using System.Linq;
using System.Text;
using Pizza.Persistence.Attributes;

namespace Pizza.Framework.DataGeneration.SpecimenBuilders
{
    internal sealed class FixedLengthStringsSpecimenBuilder : StringSpecimenBuilderBase
    {
        protected override Type[] AttributeTypes => new[] { typeof(FixedLengthAnsiStringAttribute), typeof(FixedLengthUnicodeStringAttribute) };

        protected override string BuildString(string prefix, int length)
        {
            var sb = new StringBuilder(prefix);
            while (sb.Length < length)
            {
                sb.Append(Guid.NewGuid());
            }

            return new string(sb.ToString().Take(length).ToArray());
        }
    }

}