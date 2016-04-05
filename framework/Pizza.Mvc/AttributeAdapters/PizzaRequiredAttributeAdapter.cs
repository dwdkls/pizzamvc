using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Pizza.Mvc.Resources;

namespace Pizza.Mvc.AttributeAdapters
{
    public class PizzaRequiredAttributeAdapter : RequiredAttributeAdapter
    {
        public PizzaRequiredAttributeAdapter(ModelMetadata metadata, ControllerContext context, RequiredAttribute attribute)
            : base(metadata, context, attribute)
        {
            if (string.IsNullOrEmpty(attribute.ErrorMessage) && string.IsNullOrEmpty(attribute.ErrorMessageResourceName))
            {
                attribute.ErrorMessage = Errors.DefaultModelBinder_ValueRequired;
            }
        }
    }
}