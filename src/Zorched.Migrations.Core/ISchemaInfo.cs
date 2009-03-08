using System.Collections.Generic;
using Zorched.Migrations.Framework;

namespace Zorched.Migrations.Core
{
    public interface ISchemaInfo
    {

        IDriver Driver { get; set; }

        void EnsureSchemaTable();

        void CreateSchemaTable();

        long CurrentSchemaVersion();

        List<long> AppliedMigrations();

        void InsertSchemaVersion(long version, string name);

        void DeleteSchemaVersion(long version);
    }
}