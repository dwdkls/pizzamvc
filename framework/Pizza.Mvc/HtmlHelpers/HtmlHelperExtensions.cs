using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Pizza.Mvc.Controllers;

namespace Pizza.Mvc.HtmlHelpers
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString ActionLink<TController>(this HtmlHelper htmlHelper, 
            string linkText, Expression<Func<TController, ActionResult>> expression, object routeValues = null, object htmlAttributes = null)
            where TController : Controller
        {
            string actionName = ControllerHelper.GetActionName(expression);
            string controllerName = ControllerHelper.GetName<TController>();

            return htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes);
        }
    }
}
