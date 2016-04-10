using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Omu.ValueInjecter.Injections;
using Pizza.Utils;

namespace Pizza.Framework.ValueInjection.CustomInjections
{
    internal class ViewModelToPersistenceModelInjection : PropertyInjection
    {
        public ViewModelToPersistenceModelInjection(Type sourceType)
        {
            var nonEditableProps = sourceType.GetProperties()
                .Where(IsEditableProperty);

            var propNames = nonEditableProps.Select(p => p.Name).ToArray();

            this.ignoredProps = propNames;
        }

        private static bool IsEditableProperty(PropertyInfo propertyInfo)
        {
            var editable = propertyInfo.GetAttribute<EditableAttribute>();

            return editable != null && !editable.AllowEdit;
        }
    }
}