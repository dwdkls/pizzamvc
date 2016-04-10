using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using Pizza.Utils;

namespace Pizza.Framework.Tests.Utils
{
    [TestFixture]
    public class ObjectHelperTests
    {
        internal sealed class TestViewModel
        {
            [Display(Name = "Tytuł", Description = "Coś jakiegoś")]
            public string Title { get; set; }
            [Display(Name = "Typ", Description = "Coś")]
            public string TypeName { get; set; }

            public string SuperMethod(int x, string y, double z)
            {
                return null;
            }
        }

        [Test]
        public void Test__GetPropertyAttributeValues__Working()
        {
            var name = AttributesHelper.GetPropertyAttributeValue<TestViewModel, DisplayAttribute, string>(x => x.Title, a => a.Name);
            var description = AttributesHelper.GetPropertyAttributeValue<TestViewModel, DisplayAttribute, string>(x => x.TypeName, a => a.Description);

            Assert.That(name, Is.EqualTo("Tytuł"));
            Assert.That(description, Is.EqualTo("Coś"));
        }
    }
}
