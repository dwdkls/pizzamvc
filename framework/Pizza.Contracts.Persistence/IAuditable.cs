using System;

namespace Pizza.Contracts.Persistence
{
    public interface IAuditable
    {
        int CreatedBy { get; set; }
        DateTime CreatedTime { get; set; }
        int ChangedBy { get; set; }
        DateTime ChangedTime { get; set; }
    }
}