using System.Reflection;
using Autofac;
using Autofac.Integration.Mvc;

namespace KebabManager.Web
{
    public class AutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
        }
    }
}