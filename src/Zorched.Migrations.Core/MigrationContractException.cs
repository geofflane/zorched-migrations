using System;

namespace Zorched.Migrations.Core
{
    public class MigrationContractException : Exception
    {
        public MigrationContractException(string message) : base(message) { }

        public MigrationContractException(string message, Type type) : this(message)
        {
            OffendingType = type;
        }

        public Type OffendingType { get; set; }
    }
}