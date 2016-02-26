using System;
using Pizza.Persistence.Attributes;

namespace Pizza.Persistence.Default
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