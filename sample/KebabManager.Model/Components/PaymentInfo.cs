using System;
using KebabManager.Contracts.Common;
using Pizza.Contracts.Persistence.Attributes;

namespace KebabManager.Model.Components
{
    [Component]
    public class PaymentInfo
    {
        public virtual DateTime OrderedDate { get; set; }
        public virtual DateTime? PaymentDate { get; set; }
        public virtual DateTime? CompletedDate { get; set; }
        public virtual DateTime? CancellationDate { get; set; }
        public virtual PaymentState State { get; set; }

        public virtual int Number { get; set; }
        public virtual double Double { get; set; }

        [AllowNull, UnicodeString(100)]
        public virtual string ExternalPaymentId { get; set; }
    }
}