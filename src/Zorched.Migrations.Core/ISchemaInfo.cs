using System.Collections.Generic;
using Zorched.Migrations.Framework;

namespace Zorched.Migrations.Core
{
    public interface ISchemaInfo
    {

        IDriver Driver { get; set; }

        void CreateSchemaTable();

        long CurrentSchemaVersion();

        List<long> AppliedMigrations();

        void InsertSchemaVersion(long version);

        void DeleteSchemaVersion(long version);
    }
}