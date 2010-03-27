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
using System.Data;
using Zorched.Migrations.Framework.Data;
using Zorched.Migrations.Framework.Inspection;
using Zorched.Migrations.Framework.Schema;

namespace Zorched.Migrations.Framework
{
    /// <summary>
    /// The IDriver interface is the main interface that will be called by
    /// migrations to perform actions on the database.
    /// </summary>
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