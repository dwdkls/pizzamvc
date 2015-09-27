using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Pizza.Mvc.Controllers;

namespace Pizza.Mvc.HtmlHelpers
{
    public static class MenuHtmlHelper
    {
        public static MvcHtmlString MenuLink<TController>(this HtmlHelper htmlHelper, 
            string linkText, Expression<Func<TController, ActionResult>> expression, object routeValues = null, object htmlAttributes = null) 
            where TController : Controller
        {
            string actionName = ControllerHelper.GetActionName(expression);
            string controllerName = ControllerHelper.GetName<TController>();

            return htmlHelper.MenuLink(linkText, actionName, controllerName, routeValues, htmlAttributes);
        }

        private static MvcHtmlString MenuLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, 
            object routeValues = null, object htmlAttributes = null)
        {
            // TODO: dodać current do html attributes albo w ogóle sprawdzić, czy nie da się połączyc tych dwóch metod 

            string currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("action");
            string currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");
            if (actionName == currentAction && controllerName == currentController)
            {
                return htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, new { @class = "current" });
            }

            return htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes);
        }
    }
}