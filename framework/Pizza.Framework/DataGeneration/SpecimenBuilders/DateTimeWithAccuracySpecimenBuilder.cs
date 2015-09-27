using System;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;

namespace Pizza.Framework.DataGeneration.SpecimenBuilders
{
    public class DateTimeWithAccuracySpecimenBuilder : ISpecimenBuilder
    {
        private readonly long accuracy;

        private readonly RandomNumericSequenceGenerator randomizer;

        public DateTimeWithAccuracySpecimenBuilder()
            : this(DateTime.Today.AddYears(-2), DateTime.Today.AddYears(2), 10000000)
        {
        }

        public DateTimeWithAccuracySpecimenBuilder(DateTime minDate, DateTime maxDate, long accuracy)
        {
            if (minDate >= maxDate)
                throw new ArgumentException("The 'minDate' argument must be less than the 'maxDate'.");

            this.accuracy = accuracy;

            this.randomizer = new RandomNumericSequenceGenerator(minDate.Ticks, maxDate.Ticks);
        }

        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            if (!IsNotDateTimeRequest(request))
                return this.CreateRandomDate(context);

            return new NoSpecimen(request);
        }

        private static bool IsNotDateTimeRequest(object request)
        {
            return !typeof(DateTime).IsAssignableFrom(request as Type);
        }

        private object CreateRandomDate(ISpecimenContext context)
        {
            return new DateTime(this.GetRandomNumberOfTicks(context));
        }

        private long GetRandomNumberOfTicks(ISpecimenContext context)
        {
            long randomValue = (long)this.randomizer.Create(typeof(long), context);
            long correctValue = randomValue / this.accuracy * this.accuracy;

            return correctValue;
        }
    }
}