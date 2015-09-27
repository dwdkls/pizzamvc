using Pizza.Contracts.Persistence;
using Pizza.Framework.DataGeneration.SpecimenBuilders;
using Ploeh.AutoFixture;

namespace Pizza.Framework.DataGeneration
{
    public class FixtureFactory
    {
        public static Fixture Build()
        {
            var fixture = new Fixture();
            // don't set values handled by NHibernate
            fixture.Customizations.Add(new DisablePropertiesSpecimenBuilder<IPersistenceModel>());
            fixture.Customizations.Add(new DisablePropertiesSpecimenBuilder<IVersionable>());
            fixture.Customizations.Add(new DisablePropertiesSpecimenBuilder<IAuditable>());
            fixture.Customizations.Add(new DisablePropertiesSpecimenBuilder<ISoftDeletable>());

            // handle persistence attributes
            fixture.Customizations.Add(new FixedLengthStringsSpecimenBuilder());
            fixture.Customizations.Add(new UnicodeStringSpecimenBuilder());

            // handle standard data types in non standard way
            fixture.Customizations.Add(new DateTimeWithAccuracySpecimenBuilder());

            return fixture;
        }
    }
}