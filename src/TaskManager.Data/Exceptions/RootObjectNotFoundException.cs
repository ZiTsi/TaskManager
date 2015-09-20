using System;

namespace TaskManager.Data.Exceptions
{
    [Serializable]
    public class RootObjectNotFoundException : Exception
    {
        public RootObjectNotFoundException(string message) : base(message)
        {
        }
    }
}