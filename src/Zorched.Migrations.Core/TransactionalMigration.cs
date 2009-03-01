using System;
using Zorched.Migrations.Framework;

namespace Zorched.Migrations.Core
{
    public class TransactionalMigration : Migration
    {
        public TransactionalMigration(Type t) : base(t) {}

        public override void Up(IDriver driver, ILogger logger, ISchemaInfo schemaInfo)
        {
            using (var trans = driver.Connection.BeginTransaction())
            {
                base.Up(driver, logger, schemaInfo);
                trans.Commit();
            }
        }

        public override void Down(IDriver driver, ILogger logger, ISchemaInfo schemaInfo)
        {
            using (var trans = driver.Connection.BeginTransaction())
            {
                base.Down(driver, logger, schemaInfo);

                trans.Commit();
            }
        }
    }
}