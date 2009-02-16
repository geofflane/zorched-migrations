using System;
using System.Data;
using Zorched.Migrations.Framework.Data;
using Zorched.Migrations.Framework.Schema;

namespace Zorched.Migrations.Framework
{
    public interface IDriver
    {
        void AddColumn(Action<IAddColumnOperation> fn);
        void AddForeignKey(Action<IAddForeignKeyOperation> fn);
        void AddTable(Action<IAddTableOperation> fn);

        void Drop(Action<IDropTableOperation> fn);
        void DropColumn(Action<IDropColumnOperation> fn);
        void DropConstraint(Action<IDropConstraintOperation> fn);

        void Insert(Action<IInsertOperation> fn);
        void Update(Action<IUpdateOperation> fn);
        IDataReader Select(Action<IGenericReaderOperation> fn);

        void Execute(Action<IGenericOperation> fn);

        void Execute(IOperation op);
    }
}