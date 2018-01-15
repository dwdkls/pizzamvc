using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Pizza.Framework.Operations.InternalUtils.RuntimeMetadata;
using Pizza.Framework.Operations.InternalUtils.RuntimeMetadata.Types;
using Pizza.Framework.TestTypes.Model.PersistenceModels;
using Pizza.Framework.TestTypes.ViewModels;
using Pizza.Framework.TestTypes.ViewModels.Orders;
using Pizza.Utils;

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
                { nameof(OrderGridModel.CustomerFirstName), "Customer.FirstName" },
                { nameof(OrderGridModel.CustomerLastName), "Customer.LastName" },
                { nameof(OrderGridModel.CustomerFingersCount), "Customer.FingersCount" },
                { nameof(OrderGridModel.CustomerHairLength), "Customer.HairLength" },
                { nameof(OrderGridModel.CustomerPreviousSurgeryDate), "Customer.PreviousSurgeryDate" },
                { nameof(OrderGridModel.CustomerType), "Customer.Type" },
            };

            var components = new Dictionary<string, string> {
                { nameof(OrderGridModel.PaymentInfoOrderedDate), "PaymentInfo.OrderedDate" },
                { nameof(OrderGridModel.PaymentInfoState), "PaymentInfo.State" },
                { nameof(OrderGridModel.PaymentInfoDouble), "PaymentInfo.Double" },
                { nameof(OrderGridModel.PaymentInfoExternalPaymentId), "PaymentInfo.ExternalPaymentId" },
                { nameof(OrderGridModel.PaymentInfoNumber), "PaymentInfo.Number" },
            };

            var simpleProps = new Dictionary<string, string> {
                { nameof(OrderGridModel.Id), "Id" },
                { nameof(OrderGridModel.Type), "Type" },
                { nameof(OrderGridModel.OrderDate), "OrderDate" },
                { nameof(OrderGridModel.TotalPrice), "TotalPrice" },
                { nameof(OrderGridModel.Note), "Note" },
                { nameof(OrderGridModel.ItemsCount), "ItemsCount" },
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