using System.Linq;
using System.Reflection;
using Pizza.Framework.Utils;
using Ploeh.AutoFixture.Kernel;

namespace Pizza.Framework.DataGeneration.SpecimenBuilders
{
    internal class DisablePropertiesSpecimenBuilder<T> : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            var propertyInfo = request as PropertyInfo;
            var names = ObjectHelper.GetAllPropertiesNames<T>();

            if (propertyInfo != null && names.Contains(propertyInfo.Name))
            {
                return new OmitSpecimen();
            }

            return new NoSpecimen(request);
        }
    }
}