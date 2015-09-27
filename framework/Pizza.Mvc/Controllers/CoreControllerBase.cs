using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Pizza.Mvc.Controllers
{
    public class CoreControllerBase : Controller
    {
        protected static string GetActionName(Expression<Action<ActionResult>> action)
        {
            var method = (MethodCallExpression)action.Body;
            return method.Method.Name;
        }

        protected void ShowError(string message)
        {
            this.TempData[ScriptKeys.Error] = message;
        }
    }
}