using System.Net;
using System.Net.Mime;
using System.Web.Mvc;

namespace Pizza.Mvc.Filters
{
    // TODO: refactor variables
    public class UniversalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
            {
                return;
            }

            string message = filterContext.Exception.Message;
            if (filterContext.Exception.InnerException != null)
            {
                message = $"{filterContext.Exception.Message} {filterContext.Exception.InnerException.Message}";
            }
            message = message.Replace("\r\n", " ");

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = filterContext.RequestContext.HttpContext.Response;
                response.Write(message);
                response.ContentType = MediaTypeNames.Text.Plain;
            }
            else
            {
                filterContext.HttpContext.Response.Clear();
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                filterContext.Controller.TempData[ScriptKeys.Error] = message;

                filterContext.Result = new ViewResult
                {
                    TempData = filterContext.Controller.TempData,
                    ViewData = filterContext.Controller.ViewData,
                };
            
            }

            filterContext.ExceptionHandled = true;
        }
    }
}
