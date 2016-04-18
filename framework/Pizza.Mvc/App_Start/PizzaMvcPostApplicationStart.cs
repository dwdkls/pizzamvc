using Autofac;
using Autofac.Integration.Mvc;
using Pizza.Mvc;
using Pizza.Mvc.AttributeAdapters;
using Pizza.Mvc.Filters;
using Pizza.Mvc.Helpers;
using RazorGenerator.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using Pizza.Mvc.Binders;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(PizzaMvcPostApplicationStart), "Start")]

namespace Pizza.Mvc
{
    public static class PizzaMvcPostApplicationStart
    {
        public static void Start()
        {
            RegisterRazorGeneratedViewEngine();
            RegisterGlobalFilters();
            BootstrapAutofac();

            ModelBinders.Binders.Add(typeof(DateTime), new DateTimeModelBinder());

            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(RequiredAttribute), typeof(PizzaRequiredAttributeAdapter));
        }

        private static void RegisterRazorGeneratedViewEngine()
        {
            var mainEngine = new PrecompiledMvcEngine(typeof(PizzaMvcPostApplicationStart).Assembly)
            {
                UsePhysicalViewsIfNewer = HttpContext.Current.Request.IsLocal,
            };

            ViewEngines.Engines.Insert(0, mainEngine);
            VirtualPathFactoryManager.RegisterVirtualPathFactory(mainEngine);

            var areaNames = RouteTableHelper.GetApplicationAreaNames();
            foreach (var areaName in areaNames)
            {
                var engine = new PrecompiledMvcEngine(typeof(PizzaMvcPostApplicationStart).Assembly, string.Format("~/Areas/{0}/", areaName))
                {
                    UsePhysicalViewsIfNewer = HttpContext.Current.Request.IsLocal,
                };

                ViewEngines.Engines.Insert(0, engine);
                VirtualPathFactoryManager.RegisterVirtualPathFactory(engine);
            }
        }

        private static void RegisterGlobalFilters()
        {
            GlobalFilters.Filters.Add(new UniversalExceptionFilter());
        }

        private static void BootstrapAutofac()
        {
            var builder = new ContainerBuilder();
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            builder.RegisterAssemblyModules(loadedAssemblies);
            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
