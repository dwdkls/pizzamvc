using Autofac;
using Autofac.Integration.Mvc;
using Pizza.Framework;
using Pizza.Mvc;
using Pizza.Mvc.Filters;
using Pizza.Mvc.Helpers;
using RazorGenerator.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using Pizza.Mvc.AttributeAdapters;

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

            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(RequiredAttribute), typeof(PizzaRequiredAttributeAdapter));
        }

        private static void RegisterRazorGeneratedViewEngine()
        {
            var areaNames = RouteTableHelper.GetApplicationAreaNames();
            foreach (var areaName in areaNames)
            {
                var engine = new PrecompiledMvcEngine(typeof(PizzaMvcPostApplicationStart).Assembly, string.Format("~/Areas/{0}/", areaName))
                {
                    UsePhysicalViewsIfNewer = HttpContext.Current.Request.IsLocal,
                };

                ViewEngines.Engines.Add(engine);
                VirtualPathFactoryManager.RegisterVirtualPathFactory(engine);
            }

            var mainEngine = new PrecompiledMvcEngine(typeof(PizzaMvcPostApplicationStart).Assembly)
            {
                UsePhysicalViewsIfNewer = HttpContext.Current.Request.IsLocal,
            };

            ViewEngines.Engines.Add(mainEngine);
            VirtualPathFactoryManager.RegisterVirtualPathFactory(mainEngine);
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
            PizzaServerContext.Initialize(container);
        }
    }
}
