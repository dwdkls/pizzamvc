using System;
using System.Linq;
using System.Text;
using Pizza.Contracts.Persistence.Attributes;

namespace Pizza.Framework.DataGeneration.SpecimenBuilders
{
    internal sealed class FixedLengthStringsSpecimenBuilder : StringSpecimenBuilderBase
    {
        private static readonly Type[] MyAttributeTypes = new[] { typeof(FixedLengthAnsiStringAttribute), typeof(FixedLengthUnicodeStringAttribute) };

        protected override Type[] AttributeTypes
        {
            get { return MyAttributeTypes; }
        }

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