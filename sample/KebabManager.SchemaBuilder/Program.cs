using System;
using System.Configuration;
using System.Linq;
using KebabManager.Model.PersistenceModels;
using NHibernate.Tool.hbm2ddl;
using Pizza.Framework.DataGeneration;
using Pizza.Framework.Persistence;
using Pizza.Framework.Persistence.Extensions;
using Pizza.Framework.Security;
using Pizza.Framework.Utils;
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

            var users = CreateUsers();

            var sessionFactory = configuration.BuildSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.SaveMany(users);
                    session.SaveMany(customers);
                    session.SaveMany(orders);
                    transaction.Commit();
                }
            }

            Console.WriteLine("Done!");
            Console.ReadLine();
        }

        private static User[] CreateUsers()
        {
            var user1 = new User
            {
                UserName = "john",
                FirstName = "John",
                LastName = "Smith",
                Password = HashGenerator.Generate("qwe123")
            };

            var user2 = new User
            {
                UserName = "jack",
                FirstName = "Jack",
                LastName = "Black",
                Password = HashGenerator.Generate("qwe123")
            };

            var user3 = new User
            {
                UserName = "paul",
                FirstName = "Paul",
                LastName = "Blue",
                Password = HashGenerator.Generate("qwe123")
            };

            var users = new[] {user1, user2, user3};
            return users;
        }
    }
}
