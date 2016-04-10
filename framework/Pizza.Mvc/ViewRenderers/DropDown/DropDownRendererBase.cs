using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Pizza.Mvc.Helpers;
using Pizza.Mvc.Resources;
using Pizza.Utils;

namespace Pizza.Mvc.ViewRenderers.DropDown
{
    internal abstract class DropDownRendererBase
    {
        public string RenderDropDownForEnum(HtmlHelper htmlHelper, PropertyInfo property, object value)
        {
            var items = GetDropDownItems(property);

            var dropDownHtml = htmlHelper
                .DropDownList(property.Name, items, UiTexts.Editor_DropDown_EmptyValue, new { @class = "form-control" })
                .ToHtmlString();
            
            dropDownHtml = this.SelectProperItem(dropDownHtml, value);

            if (property.GetAttribute<RequiredAttribute>() == null)
            {
                dropDownHtml = dropDownHtml.Replace("data-val=\"true\"", string.Empty);
            }

            var validationMessage = htmlHelper.ValidationMessage(property.Name, string.Empty, new { @class = "text-danger" });
            if (validationMessage != null)
            {
                dropDownHtml += validationMessage.ToHtmlString();
            }

            return dropDownHtml;
        }

        protected abstract string SelectProperItem(string dropDownHtml, object value);

        private static List<SelectListItem> GetDropDownItems(PropertyInfo property)
        {
            var enumValues = EnumDisplayNameHelper.GetValueNameMap(property.PropertyType);
            var items = enumValues.Select(enumValue => new SelectListItem
            {
                Value = enumValue.Key.ToString(CultureInfo.InvariantCulture),
                Text = enumValue.Value,
            }).ToList();

            return items;
        }
    }
}