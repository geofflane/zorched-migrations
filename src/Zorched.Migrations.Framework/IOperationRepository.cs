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
using Zorched.Migrations.Framework.Data;
using Zorched.Migrations.Framework.Inspection;

namespace Zorched.Migrations.Framework
{
    /// <summary>
    /// Maintains a mapping of Interfaces to their implementation classes.
    /// </summary>
    /// <remarks>
    /// This will allow users to override the default implementations as well
    /// as create their own interfaces and implementations.
    /// </remarks>
    public interface IOperationRepository
    {
        void Register<T>(Type impl) where T : IOperation;
        void RegisterReader<T>(Type impl) where T : IReaderOperation;
        void RegisterInspecor<T>(Type impl) where T : IInspectionOperation;

        T InstanceForInteface<T>();
    }
}