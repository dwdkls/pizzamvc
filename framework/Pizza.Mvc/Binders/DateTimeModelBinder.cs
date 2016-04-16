using System;
using System.Globalization;
using System.Web.Mvc;

namespace Pizza.Mvc.Binders
{
    public class DateTimeModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var conversionCulture = CultureInfo.CurrentCulture.Name == "pl-PL" ? new CultureInfo("de-DE") : CultureInfo.CurrentCulture;
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            var modelState = new ModelState { Value = valueProviderResult };

            object resultValue = null;
            try
            {
                resultValue = Convert.ToDateTime(valueProviderResult.AttemptedValue, conversionCulture);
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