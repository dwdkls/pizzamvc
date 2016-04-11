using Autofac;
using KebabManager.Model.PersistenceModels;
using Pizza.Framework.Persistence;
using System.Configuration;

namespace KebabManager.Application
{
    public class AutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            builder.RegisterPersistence(connectionString, typeof(Customer).Assembly)
                .RegisterApplicationServices(this.ThisAssembly);
        }
    }
}
