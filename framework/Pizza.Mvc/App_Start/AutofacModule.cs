using System.Reflection;
using Autofac;
using Autofac.Integration.Mvc;
using Pizza.Contracts.Security;
using Pizza.Mvc.Security;

namespace Pizza.Mvc
{
    public class AutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DefaultApplicationUserContext>().As<IPizzaUserContext>();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
        }
    }
}