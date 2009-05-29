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
    /// The attribute that will mark the Driver class in an Assembly
    /// for a specific implementation. e.g. SQLServer, MySQL, etc.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DriverAttribute : Attribute
    {
        public DriverAttribute(string name, string provider)
        {
            Name = name;
            Provider = provider;
        }

        public string Name { get; set; }
        public string Provider { get; set; }

        public static Type GetDriver(Assembly assembly)
        {
            var types = assembly.GetTypesWithAttribute(typeof (DriverAttribute));
            var enumerator = types.GetEnumerator();
            if (! enumerator.MoveNext())
                throw new Exception("No driver found.");

            return enumerator.Current;
        }

        public static string GetDriverName(Type t)
        {
            return GetAttribute(t).Name;
        }

        public static string GetProvider(Type t)
        {
            return GetAttribute(t).Provider;
        }

        private static DriverAttribute GetAttribute(Type t)
        {
            return (DriverAttribute)GetCustomAttribute(t, typeof(DriverAttribute), true);
        }
    }
}