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

namespace Zorched.Migrations.Framework
{
    /// <summary>
    /// This attribute will mark a Migration method as something that should be run
    /// only when the currently running Driver type matches one of the Drivers in 
    /// the list supplied.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class OnlyWhenDriverAttribute : Attribute
    {
        public OnlyWhenDriverAttribute(string drivers)
        {
            Drivers = new List<string>(drivers.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
        }

        public IList<string> Drivers { get; set; }

        public static bool ShouldRun(MethodInfo mi, string driverName)
        {
            if (! HasAttribute(mi))
                return true;

            return IncludesDriver(mi, driverName);
        }

        public static bool HasAttribute(MethodInfo mi)
        {
            return null != GetAttribute(mi);
        }

        public static IList<string> GetDriverNames(MethodInfo mi)
        {
            return GetAttribute(mi).Drivers;
        }

        public static bool IncludesDriver(MethodInfo mi, string driverName)
        {
            return GetAttribute(mi).Drivers.Any(d => d == driverName);
        }

        private static OnlyWhenDriverAttribute GetAttribute(MemberInfo mi)
        {
            return (OnlyWhenDriverAttribute)GetCustomAttribute(mi, typeof(OnlyWhenDriverAttribute), true);   
        }
    }
}