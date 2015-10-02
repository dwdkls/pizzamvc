using System;
using System.Configuration;
using System.Linq;
using KebabManager.Model.PersistenceModels;
using NHibernate.Tool.hbm2ddl;
using Pizza.Framework.DataGeneration;
using Pizza.Framework.Persistence;
using Pizza.Framework.Persistence.Extensions;
using Ploeh.AutoFixture;

namespace KebabManager.SchemaBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            var configuration = NhConfigurationFactory.BuildConfiguration(connectionString, typeof(Customer).Assembly);

            var exporter = new SchemaExport(configuration);
            exporter.Create(true, true);
            Console.WriteLine("Created!");

            var fixture = FixtureFactory.Build();

            var customers = fixture.CreateMany<Customer>(27).ToList();
            var orders = fixture.CreateMany<Order>(27 * 13)
                .SetRandomValuesForAllItems(o => o.Customer, customers);

            var sessionFactory = configuration.BuildSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.SaveMany(customers);
                    session.SaveMany(orders);
                    transaction.Commit();
                }
            }

            Console.WriteLine("Done!");
            Console.ReadLine();
        }
    }
}
