using System;
using Zorched.Migrations.Framework.Schema;

namespace Zorched.Migrations.Framework
{
    public interface IDriver
    {
        void ChangeSchema(ISchemaOperation op);

        void Drop(Action<IDropTableOperation> fn);
        void AddTable(Action<IAddTableOperation> fn);
    }
}