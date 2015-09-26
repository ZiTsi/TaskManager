using System;

namespace TaskManager.Common
{
    public class BusinessRuleViolationException : Exception
    {
        public BusinessRuleViolationException(string incorrectTaskStatus) : base(incorrectTaskStatus)
        {
        }
    }
}