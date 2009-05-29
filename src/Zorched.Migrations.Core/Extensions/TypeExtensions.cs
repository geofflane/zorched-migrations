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
using System.Linq;
using System.Reflection;
using Zorched.Migrations.Framework.Extensions;

namespace Zorched.Migrations.Core.Extensions
{
    public static class TypeExtensions
    {
        public static IEnumerable<MethodInfo> GetMethodsWithAttribute(this Type t, Type attributeType)
        {
            var methods = t.GetMethods();
            return methods.Where(m => Attribute.IsDefined(m, attributeType, true));
        }

        /// <summary>
        /// Make a Human readable name from the name of the Type.
        /// </summary>
        /// <example>
        /// typeof(AddPersonTable).ToHumanName() == "Add person table"
        /// </example>
        /// <param name="t">The type that you want the name for.</param>
        /// <returns>A name that has spaces where there are capital letters.</returns>
        public static string ToHumanName(this Type t)
        {
            return t.Name.ToHumanName();
        }
    }
}