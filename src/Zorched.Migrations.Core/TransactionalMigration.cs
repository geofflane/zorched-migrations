using System;
using Zorched.Migrations.Framework;

namespace Zorched.Migrations.Core
{
    public class TransactionalMigration : Migration
    {
        public TransactionalMigration(Type t) : base(t) {}

        public override void Up(IDriver driver, ISchemaInfo schemaInfo)
        {
            using (var trans = driver.Connection.BeginTransaction())
            {
                base.Up(driver, schemaInfo);
                trans.Commit();
            }
        }

        public override void Down(IDriver driver, ISchemaInfo schemaInfo)
        {
            using (var trans = driver.Connection.BeginTransaction())
            {
                base.Down(driver, schemaInfo);

                trans.Commit();
            }
        }
    }
}