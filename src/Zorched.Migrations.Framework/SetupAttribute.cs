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
using System.Reflection;
using Zorched.Migrations.Framework.Extensions;

namespace Zorched.Migrations.Framework
{
    /// <summary>
    /// An attrbute that can mark a class or a method in a Migration as something that
    /// should be run once prior to any Up or Down methods.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class SetupAttribute : Attribute
    {
        public static Type GetSetupClass(Assembly assembly)
        {
            var types = assembly.GetTypesWithAttribute(typeof(SetupAttribute));
            var enumerator = types.GetEnumerator();
            if (enumerator.MoveNext())
                return enumerator.Current;

            return null;
        }
    }
}