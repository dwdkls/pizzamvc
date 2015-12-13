using System;
using Pizza.Contracts.Persistence.Attributes;

namespace Pizza.Contracts.Persistence.Default
{
    [Component]
    public class AuditInfo : IAuditable
    {
        public virtual int CreatedBy { get; set; }
        public virtual DateTime CreatedTime { get; set; }
        public virtual int ChangedBy { get; set; }
        public virtual DateTime ChangedTime { get; set; }
    }
}