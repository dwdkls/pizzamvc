using Autofac;
using Autofac.Integration.Mvc;
using Pizza.Framework;
using Pizza.Mvc;
using Pizza.Mvc.Filters;
using Pizza.Mvc.Helpers;
using RazorGenerator.Mvc;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.WebPages;

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

            //BundleConfig.RegisterBundles(BundleTable.Bundles);
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

    //public class BundleConfig
    //{
    //    // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
    //    public static void RegisterBundles(BundleCollection bundles)
    //    {
    //        bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
    //            "~/Scripts/jquery-{version}.js"
    //        ));

    //        bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
    //            "~/Scripts/jquery-ui-{version}.js",
    //            "~/Scripts/i18n/grid.locale-en.js",
    //            "~/Scripts/jquery.jqGrid.js"
    //        ));

    //        bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
    //            "~/Scripts/jquery.unobtrusive*",
    //            "~/Scripts/jquery.validate*"
    //        ));

    //        bundles.Add(new ScriptBundle("~/bundles/tinymce").Include(
    //            "~/Scripts/tinymce/tinymce.js",
    //            "~/Scripts/tinymce/jquery.tinymce.min.js"
    //        ));

    //        // Use the development version of Modernizr to develop with and learn from. Then, when you're
    //        // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
    //        bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
    //            "~/Scripts/modernizr-*"));

    //        bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
    //            "~/Scripts/bootstrap.js",
    //            "~/Scripts/bootstrap-datepicker.js",
    //            "~/Scripts/respond.js"));

    //        bundles.Add(new StyleBundle("~/Content/css").Include(
    //            "~/Content/bootstrap.css",
    //            "~/Content/bootstrap-datepicker3.css",

    //            "~/Content/jquery.ui.theme.css",

    //            "~/Content/Pizza.css",

    //            "~/Content/jquery.jqGrid/ui.jqgrid.css",
    //            "~/Content/jqGrid.bootstrap.css"

    //        //"~/Content/themes/base/jquery.ui.core.css",
    //        //"~/Content/themes/base/jquery.ui.resizable.css",
    //        //"~/Content/themes/base/jquery.ui.selectable.css",
    //        //"~/Content/themes/base/jquery.ui.accordion.css",
    //        //"~/Content/themes/base/jquery.ui.autocomplete.css",
    //        //"~/Content/themes/base/jquery.ui.button.css",
    //        //"~/Content/themes/base/jquery.ui.dialog.css",
    //        //"~/Content/themes/base/jquery.ui.slider.css",
    //        //"~/Content/themes/base/jquery.ui.tabs.css",
    //        //"~/Content/themes/base/jquery.ui.datepicker.css",
    //        //"~/Content/themes/base/jquery.ui.progressbar.css",
    //        //"~/Content/themes/base/jquery.ui.theme.css"
    //        ));
    //    }
    //}
}
