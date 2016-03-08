using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Pizza.Framework.Operations.InternalUtils.RuntimeMetadata;
using Pizza.Framework.Operations.InternalUtils.RuntimeMetadata.Types;
using Pizza.Framework.TestTypes.Model.PersistenceModels;
using Pizza.Framework.TestTypes.ViewModels;
using Pizza.Framework.TestTypes.ViewModels.Orders;
using Pizza.Framework.Utils;

namespace Pizza.Framework.Tests.Operations.Utils
{
    [TestFixture]
    public class ViewModelToPersistenceModelPropertyNamesMapsGeneratorTests
    {
        [Test]
        public void Generate__GeneratedProperly()
        {
            // Prepare expected results
            var joinedModels = new Dictionary<string, string> {
                { ObjectHelper.GetPropertyName<OrderGridModel>(v => v.CustomerFirstName), "Customer.FirstName" },
                { ObjectHelper.GetPropertyName<OrderGridModel>(v => v.CustomerLastName), "Customer.LastName" },
                { ObjectHelper.GetPropertyName<OrderGridModel>(v => v.CustomerFingersCount), "Customer.FingersCount" },
                { ObjectHelper.GetPropertyName<OrderGridModel>(v => v.CustomerHairLength), "Customer.HairLength" },
                { ObjectHelper.GetPropertyName<OrderGridModel>(v => v.CustomerPreviousSurgeryDate), "Customer.PreviousSurgeryDate" },
                { ObjectHelper.GetPropertyName<OrderGridModel>(v => v.CustomerType), "Customer.Type" },
            };

            var components = new Dictionary<string, string> {
                { ObjectHelper.GetPropertyName<OrderGridModel>(v => v.PaymentInfoOrderedDate), "PaymentInfo.OrderedDate" },
                { ObjectHelper.GetPropertyName<OrderGridModel>(v => v.PaymentInfoState), "PaymentInfo.State" },
                { ObjectHelper.GetPropertyName<OrderGridModel>(v => v.PaymentInfoDouble), "PaymentInfo.Double" },
                { ObjectHelper.GetPropertyName<OrderGridModel>(v => v.PaymentInfoExternalPaymentId), "PaymentInfo.ExternalPaymentId" },
                { ObjectHelper.GetPropertyName<OrderGridModel>(v => v.PaymentInfoNumber), "PaymentInfo.Number" },
            };

            var simpleProps = new Dictionary<string, string> {
                { ObjectHelper.GetPropertyName<OrderGridModel>(v => v.Id), "Id" },
                { ObjectHelper.GetPropertyName<OrderGridModel>(v => v.Type), "Type" },
                { ObjectHelper.GetPropertyName<OrderGridModel>(v => v.OrderDate), "OrderDate" },
                { ObjectHelper.GetPropertyName<OrderGridModel>(v => v.TotalPrice), "TotalPrice" },
                { ObjectHelper.GetPropertyName<OrderGridModel>(v => v.Note), "Note" },
                { ObjectHelper.GetPropertyName<OrderGridModel>(v => v.ItemsCount), "ItemsCount" },
            };

            var allProps = joinedModels.Union(components).Union(simpleProps).ToDictionary(x => x.Key, x => x.Value);

            // ACT
            var modelDescription = GetTestModelDescription();
            var map = ViewModelToPersistenceModelPropertyNamesMapsGenerator.Generate(typeof(OrderGridModel), modelDescription);

            // ASSERT
            map.Should(Be.Not.Null);
            map.AllProperties.Count.Should(Be.EqualTo(17));
            map.AllProperties.Should(Be.EquivalentTo(allProps));
        }

        private static PersistenceModelPropertiesDescription GetTestModelDescription()
        {
            var joinedModels = new[] { PropInfo.FromPropertyExpression<Order>(v => v.Customer) };
            var components = new[] { 
                PropInfo.FromPropertyExpression<Order>(v => v.PaymentInfo), 
                PropInfo.FromPropertyExpression<Order>(v => v.AuditInfo) 
            };
            var simpleProps = new[] {
                PropInfo.FromPropertyExpression<Order>(v => v.Id), 
                PropInfo.FromPropertyExpression<Order>(v => v.Type), 
                PropInfo.FromPropertyExpression<Order>(v => v.OrderDate),
                PropInfo.FromPropertyExpression<Order>(v => v.TotalPrice),
                PropInfo.FromPropertyExpression<Order>(v => v.Note),
                PropInfo.FromPropertyExpression<Order>(v => v.ItemsCount),
            };

            return new PersistenceModelPropertiesDescription(simpleProps, components, joinedModels);
        }
    }
}