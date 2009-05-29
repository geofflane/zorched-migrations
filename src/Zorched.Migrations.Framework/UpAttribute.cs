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

namespace Zorched.Migrations.Framework
{
    /// <summary>
    /// An attribute that will mark a method in a Migration as a method
    /// to be run when Migrating to a new, larger version of the database schema.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class UpAttribute : Attribute, IMigrationDirection
    {
        public UpAttribute()
        {
            Order = 1;
        }
        
        public UpAttribute(int order)
        {
            Order = order;
        }

        public int Order { get; set; }

        public static int GetOrder(MethodInfo mi)
        {
            var upAttr = (IMigrationDirection) GetCustomAttribute(mi, typeof(UpAttribute), true);
            return upAttr.Order;
        }
    }
}