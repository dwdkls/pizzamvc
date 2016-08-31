using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Pizza.Mvc.Controllers
{
    public class ControllerHelper
    {
        public static string GetName<TController>()
            where TController : Controller
        {
            return typeof(TController).Name.Replace("Controller", string.Empty);
        }

        public static string GetActionName<TController>(Expression<Func<TController, ActionResult>> expression)
            where TController : Controller
        {
            var method = (MethodCallExpression)expression.Body;
            return method.Method.Name;
        }
    }
}