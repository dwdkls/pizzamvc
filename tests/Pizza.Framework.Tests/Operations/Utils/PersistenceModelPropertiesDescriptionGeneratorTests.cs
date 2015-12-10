using NUnit.Framework;
using Pizza.Framework.Operations.InternalUtils.RuntimeMetadata;
using Pizza.Framework.Operations.InternalUtils.RuntimeMetadata.Types;
using Pizza.Framework.TestTypes.Model.PersistenceModels;

namespace Pizza.Framework.Tests.Operations.Utils
{
    [TestFixture]
    public class PersistenceModelPropertiesDescriptionGeneratorTests
    {
        [Test]
        public void GenerateDescription__AllPropertiesGroupedCorrectly()
        {
            var joinedEntities = new[] { PropInfo.FromPropertyExpression<Order>(v => v.Customer) };
            var components = new[] {
                PropInfo.FromPropertyExpression<Order>(v => v.PaymentInfo), 
                PropInfo.FromPropertyExpression<Order>(v => v.AuditInfo)
            };
            var simpleProps = new[] {
                PropInfo.FromPropertyExpression<Order>(v => v.Id),
                PropInfo.FromPropertyExpression<Order>(v => v.ItemsCount),
                PropInfo.FromPropertyExpression<Order>(v => v.Note),
                PropInfo.FromPropertyExpression<Order>(v => v.Type),
                PropInfo.FromPropertyExpression<Order>(v => v.OrderDate),
                PropInfo.FromPropertyExpression<Order>(v => v.TotalPrice),
                PropInfo.FromPropertyExpression<Order>(v => v.Version),
            };

            var modelDescription = PersistenceModelPropertiesDescriptionGenerator.GenerateDescription(typeof(Order));

            modelDescription.Should(Be.Not.Null);

            modelDescription.ComponentProperties.Count.Should(Be.EqualTo(components.Length));
            modelDescription.JoinedModels.Count.Should(Be.EqualTo(joinedEntities.Length));
            modelDescription.SimpleProperties.Count.Should(Be.EqualTo(simpleProps.Length));
            
            modelDescription.ComponentProperties.Should(Be.EquivalentTo(components));
            modelDescription.JoinedModels.Should(Be.EquivalentTo(joinedEntities));
            modelDescription.SimpleProperties.Should(Be.EquivalentTo(simpleProps));
        }
    }
}