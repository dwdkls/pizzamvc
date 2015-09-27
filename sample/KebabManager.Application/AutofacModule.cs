using System.Configuration;
using System.Reflection;
using Autofac;
using KebabManager.Model.PersistenceModels;
using Pizza.Framework;

namespace KebabManager.Application
{
    public class AutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            AutofacRegisterHelper.RegisterPersistenceStuffAndServices(builder, connectionString, typeof(Customer).Assembly, Assembly.GetExecutingAssembly());
        }
    }
}
