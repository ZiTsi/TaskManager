using System;

namespace TaskManager.Common
{
    public interface IDateTime
    {
        DateTime UtcNow { get; }
    }
}