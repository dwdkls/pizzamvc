using System;
using KebabManager.Common.Enums;
using Pizza.Contracts.Default.Persistence;
using Pizza.Contracts.Persistence.Attributes;

namespace KebabManager.Model.PersistenceModels
{
    public class Customer : SoftDeletableModelBase
    {
        [Unique, UnicodeString(30)]
        public virtual string Login { get; set; }

        [FixedLengthAnsiString(128)]
        public virtual string Password { get; set; }

        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual int FingersCount { get; set; }
        public virtual DateTime PreviousSurgeryDate { get; set; }
        public virtual DateTime SomeDateInFuture { get; set; }
        public virtual double HairLength { get; set; }
        public virtual CustomerType Type { get; set; }

        [AllowNull]
        public virtual AnimalSpecies? Animal { get; set; }

        [AllowNull, UnicodeString]
        public virtual string Description { get; set; }

        [AllowNull, UnicodeString]
        public virtual string Notes { get; set; }

        public virtual string FullName
        {
            get { return string.Format("{0} {1}", this.FirstName, this.LastName); }
        }
    }
}