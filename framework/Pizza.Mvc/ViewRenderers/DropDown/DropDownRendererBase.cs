using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Pizza.Framework.Utils;
using Pizza.Mvc.Helpers;

namespace Pizza.Mvc.ViewRenderers.DropDown
{
    internal abstract class DropDownRendererBase
    {
        public string RenderDropDownForEnum(HtmlHelper htmlHelper, PropertyInfo property, object value)
        {
            var items = GetDropDownItems(property);

            // TODO: move empty item text to resources
            var dropDownHtml = htmlHelper
                .DropDownList(property.Name, items, "Select value", new { @class = "form-control" })
                .ToHtmlString();

            dropDownHtml = this.SelectProperItem(dropDownHtml, value);

            if (property.GetAttribute<RequiredAttribute>() == null)
            {
                dropDownHtml = dropDownHtml.Replace("data-val=\"true\"", string.Empty);
            }
            //else
            //{
            var validationMessage = htmlHelper.ValidationMessage(property.Name, string.Empty, new { @class = "text-danger" });
            dropDownHtml += validationMessage.ToHtmlString();
            //}

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