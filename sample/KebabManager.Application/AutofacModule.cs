using Autofac;
using KebabManager.Model.PersistenceModels;
using Pizza.Framework;
using System.Configuration;
using System.Reflection;
using Pizza.Framework.Persistence;

namespace KebabManager.Application
{
    public class AutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            var configuration = NhConfigurationFactory.BuildConfiguration(connectionString, typeof(Customer).Assembly);

            AutofacRegisterHelper.RegisterPersistenceStuffAndServices(builder, configuration, typeof(Customer).Assembly, Assembly.GetExecutingAssembly());
        }
    }
}
