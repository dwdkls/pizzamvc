using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using KebabManager.Application;
using KebabManager.Web.Security;
using Pizza.Mvc;

namespace KebabManager.Web
{
    public class MvcApplication : PizzaMvcApplicationBase<KebabPrincipal, KebabPrincipalSerializeModel>
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}