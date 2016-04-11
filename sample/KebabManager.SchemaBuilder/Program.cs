using Autofac;
using KebabManager.Model.PersistenceModels;
using NHibernate.Tool.hbm2ddl;
using NSubstitute;
using Pizza.Contracts.Security;
using Pizza.Framework;
using Pizza.Framework.DataGeneration;
using Pizza.Framework.Persistence.Extensions;
using Ploeh.AutoFixture;
using System;
using System.Linq;
using NHibernate;
using NHibernate.Cfg;
using Pizza.Framework.Persistence;
using Pizza.Utils;

namespace KebabManager.SchemaBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = BuildContainer();

            CreateDatabase(container);
            Console.WriteLine("Created!");

            PopulateSampleData(container);
            Console.WriteLine("Done!");

            Console.ReadLine();
        }

        private static IContainer BuildContainer()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            var builder = new ContainerBuilder()
                .RegisterPersistence(connectionString, typeof(Customer).Assembly);

            RegisterMockedUserContext(builder);

            var container = builder.Build();
            return container;
        }

        private static void CreateDatabase(IContainer container)
        {
            var configuration = container.Resolve<Configuration>();
            var exporter = new SchemaExport(configuration);
            exporter.Create(true, true);
        }

        private static void PopulateSampleData(IContainer container)
        {
            var fixture = FixtureFactory.Build();
            var customers = fixture.CreateMany<Customer>(27).ToList();
            var orders = fixture.CreateMany<Order>(27 * 13)
                .SetRandomValuesForAllItems(o => o.Customer, customers);

            var users = CreateUsers();

            using (var session = container.Resolve<ISession>())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.SaveMany(users);
                    session.SaveMany(customers);
                    session.SaveMany(orders);
                    transaction.Commit();
                }
            }
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

            var users = new[] { user1, user2, user3 };
            return users;
        }

        private static void RegisterMockedUserContext(ContainerBuilder builder)
        {
            var pizzaPrincipal = Substitute.For<IPizzaPrincipal>();
            pizzaPrincipal.Id.Returns(997);

            var userContext = Substitute.For<IPizzaUserContext>();
            userContext.CurrentUser.Returns(pizzaPrincipal);

            builder.RegisterInstance(userContext).AsImplementedInterfaces();
        }
    }
}
