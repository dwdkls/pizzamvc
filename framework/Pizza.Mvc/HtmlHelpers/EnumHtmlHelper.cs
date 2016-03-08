using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Pizza.Framework.Utils;
using Pizza.Mvc.Helpers;

namespace Pizza.Mvc.HtmlHelpers
{
    public static class EnumHtmlHelper
    {
        public static IHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> html, Expression<Func<TModel, TEnum>> expression)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var enumType = metadata.ModelType.GetRealType();
            var enumValues = EnumDisplayNameHelper.GetValueNameMap(enumType);
            var selectedValue = (int)metadata.Model;

            var items = enumValues.Select(enumValue => new SelectListItem
            {
                Value = enumValue.Key.ToString(CultureInfo.InvariantCulture),
                Text = enumValue.Value,
                Selected = enumValue.Key == selectedValue
            });

            // TODO: pass as parameter
            return html.DropDownListFor(expression, items, "Select", null);
        }

        public static IHtmlString EnumDisplayFor<TModel, TEnum>(this HtmlHelper<TModel> html, Expression<Func<TModel, TEnum>> expression)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var enumType = metadata.ModelType.GetRealType();
            var name = EnumDisplayNameHelper.GetDisplayName(enumType, metadata.Model);

            return html.Raw(name);
        }
    }
}