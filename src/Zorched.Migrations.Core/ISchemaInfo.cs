using System.Collections.Generic;
using Zorched.Migrations.Framework;

namespace Zorched.Migrations.Core
{
    public interface ISchemaInfo
    {

        IDriver Driver { get; set; }

        void EnsureSchemaTable();

        void CreateSchemaTable();

        long CurrentSchemaVersion(string assembly);

        List<long> AppliedMigrations(string assembly);

        void InsertSchemaVersion(long version, string assembly, string name);

        void DeleteSchemaVersion(long version, string assembly);
    }
}