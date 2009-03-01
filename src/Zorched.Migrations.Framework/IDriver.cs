using System;
using System.Data;
using Zorched.Migrations.Framework.Data;
using Zorched.Migrations.Framework.Inspection;
using Zorched.Migrations.Framework.Schema;

namespace Zorched.Migrations.Framework
{
    public interface IDriver
    {
        IDbParams Database { get; }
        string DriverName { get; }

        void Run<T>(Action<T> fn) where T : IOperation;
        void Run(IOperation op);

        IDataReader Read<T>(Action<T> fn) where T : IReaderOperation;
        IDataReader Read(IReaderOperation op);

        bool Inspect<T>(Action<T> op) where T : IInspectionOperation;
        bool Inspect(IInspectionOperation op);

        void AddColumn(Action<IAddColumnOperation> fn);
        void AddForeignKey(Action<IAddForeignKeyOperation> fn);
        void AddTable(Action<IAddTableOperation> fn);

        void Drop(Action<IDropTableOperation> fn);
        void DropColumn(Action<IDropColumnOperation> fn);
        void DropConstraint(Action<IDropConstraintOperation> fn);

        void Delete(Action<IDeleteOperation> fn);
        void Insert(Action<IInsertOperation> fn);
        void Update(Action<IUpdateOperation> fn);
        IDataReader Select(Action<ISelectOperation> fn);


        void BeforeUp(long version);
        void BeforeDown(long version);

        void AfterUp(long version);
        void AfterDown(long version);
    }
}