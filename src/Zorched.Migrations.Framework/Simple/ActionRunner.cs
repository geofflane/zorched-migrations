using System;
using System.Data;
using Zorched.Migrations.Framework.Data;
using Zorched.Migrations.Framework.Inspection;
using Zorched.Migrations.Framework.Schema;

namespace Zorched.Migrations.Framework.Simple
{
    public class ActionRunner : Runner
    {
        public ActionRunner(IDriver driver) : base(driver)
        {
        }

        public IDataReader Select(Action<ISelectOperation> fn)
        {
            return Driver.Read(fn);
        }

        public void AddColumn(Action<IAddColumnOperation> fn)
        {
            Driver.Run(fn);
        }

        public void AddForeignKey(Action<IAddForeignKeyOperation> fn)
        {
            Driver.Run(fn);
        }

        public void AddTable(Action<IAddTableOperation> fn)
        {
            Driver.Run(fn);
        }

        public void Drop(Action<IDropTableOperation> fn)
        {
            Driver.Run(fn);
        }

        public void DropColumn(Action<IDropColumnOperation> fn)
        {
            Driver.Run(fn);
        }

        public void DropConstraint(Action<IDropConstraintOperation> fn)
        {
            Driver.Run(fn);
        }

        public void Delete(Action<IDeleteOperation> fn)
        {
            Driver.Run(fn);
        }

        public void Insert(Action<IInsertOperation> fn)
        {
            Driver.Run(fn);
        }

        public void Update(Action<IUpdateOperation> fn)
        {
            Driver.Run(fn);
        }

        public void Run<T>(Action<T> fn) where T : IOperation
        {
            Driver.Run(fn);
        }
        
        public IDataReader Read<T>(Action<T> fn) where T : IReaderOperation
        {
            return Driver.Read(fn);
        }

        public bool Inspect<T>(Action<T> op) where T : IInspectionOperation
        {
            return Driver.Inspect(op);
        }
    }
}