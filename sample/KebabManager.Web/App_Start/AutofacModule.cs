using Autofac;
using Autofac.Integration.Mvc;
using System.Reflection;

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