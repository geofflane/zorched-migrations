using System;

namespace Zorched.Migrations.Framework
{
    public class OperationNotSupportedException : Exception
    {
        public OperationNotSupportedException(string message) : base(message) { }
        public OperationNotSupportedException(string message, Type operation) : base(message)
        {
            Operation = operation;
        }

        public Type Operation { get; set; }
    }
}