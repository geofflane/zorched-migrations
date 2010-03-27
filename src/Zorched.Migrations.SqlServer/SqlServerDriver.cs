// Copyright 2008, 2009 Geoff Lane <geoff@zorched.net>
// 
// This file is part of Zorched Migrations a SQL Schema Migration framework for .NET.
// 
// Zorched Migrations is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Zorched Migrations is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Data;
using Zorched.Migrations.Framework;
using Zorched.Migrations.Framework.Data;
using Zorched.Migrations.Framework.Inspection;
using Zorched.Migrations.Framework.Schema;
using Zorched.Migrations.SqlServer.Data;
using Zorched.Migrations.SqlServer.Inspection;
using Zorched.Migrations.SqlServer.Schema;

namespace Zorched.Migrations.SqlServer
{
    [Driver("SQLServer", "System.Data.SqlClient")]
    public class SqlServerDriver : AbstractDriver, IOperationRepository
    {
        public SqlServerDriver(IDbParams dbParams, ILogger logger) : base(dbParams, logger)
        {
            Register<IAddTableOperation>(typeof (SqlAddTableOperation));
            Register<IAddColumnOperation>(typeof (SqlAddColumnOperation));
            Register<IAddForeignKeyOperation>(typeof(SqlAddForeignKeyOperation));
            Register<IChangeColumnOperation>(typeof(SqlChangeColumnOperation));
            Register<IDropTableOperation>(typeof (SqlDropTableOperation));
            Register<IDropColumnOperation>(typeof (SqlDropColumnOperation));
            Register<IDropConstraintOperation>(typeof(SqlDropConstraintOperation));
            Register<IRenameTableOperation>(typeof(SqlRenameTableOperation));
            Register<IRenameColumnOperation>(typeof(SqlRenameColumnOperation));
            Register<IAddCheckConstraintOperation>(typeof(SqlAddCheckConstraintOperation));
            Register<IAddUniqueConstraintOperation>(typeof(SqlAddUniqueConstraintOperation));
            Register<IGenericOperation>(typeof (SqlGenericOperation));

            Register<IDeleteOperation>(typeof (SqlDeleteOperation));
            Register<IInsertOperation>(typeof (SqlInsertOperation));
            Register<IUpdateOperation>(typeof (SqlUpdateOperation));

            RegisterReader<IGenericReaderOperation>(typeof (SqlReaderOperation));
            RegisterReader<ISelectOperation>(typeof (SqlSelectOperation));

            RegisterInspector<ITableExistsOperation>(typeof(SqlTableExistsOperation));
            RegisterInspector<IColumnExistsOperation>(typeof(SqlColumnExistsOperation));
        }
    }
}