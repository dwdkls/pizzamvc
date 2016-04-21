using System;
using System.Web.Mvc;
using Pizza.Utils;

namespace Pizza.Mvc.Binders
{
    public class DateTimeModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            var modelState = new ModelState { Value = valueProviderResult };

            object resultValue = null;
            try
            {
                resultValue = Convert.ToDateTime(valueProviderResult.AttemptedValue, CultureInfoHelper.CurrentDateTimeCulture);
            }
            catch (FormatException e)
            {
                modelState.Errors.Add(e);
            }

            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
            return resultValue;
        }
    }
}