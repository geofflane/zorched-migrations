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
using System.Text;
using System.Text.RegularExpressions;

namespace Zorched.Migrations.Framework.Extensions
{
    /// <summary>
    /// Extension methods for dealing with String types.
    /// </summary>
    public static class StringExtensions
    {
        static readonly Regex HUMANIZE_REGEX = new Regex("([A-Z])");

        /// <summary>
        /// Convert a classname to something more readable.
        /// ex.: CreateATable => Create a table
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public static string ToHumanName(this string className)
        {
            if (HUMANIZE_REGEX.IsMatch(className))
            {
                string humanName = HUMANIZE_REGEX.Replace(className, " $1");
                if (Char.IsUpper(className[0]))
                    humanName = humanName.Substring(1);     // remove the leading space we introduced
                return humanName.Substring(0, 1) + humanName.Substring(1).ToLower();
            }
            return className;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        /// <param name="placeholder"></param>
        /// <param name="replacement"></param>
        /// <returns></returns>
        public static string ReplaceOnce(this string template, string placeholder, string replacement)
        {
            int loc = template.IndexOf(placeholder);
            if (loc < 0)
            {
                return template;
            }
            else
            {
                return new StringBuilder(template.Substring(0, loc))
                    .Append(replacement)
                    .Append(template.Substring(loc + placeholder.Length))
                    .ToString();
            }
        }

        public static bool EndsWith(this StringBuilder sb, char val)
        {
            return sb[sb.Length - 1] == val;
        }

        public static StringBuilder TrimEnd(this StringBuilder sb, char val)
        {
            if (sb.EndsWith(val))
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb;
        }

        public static StringBuilder TrimEnd(this StringBuilder sb)
        {
            while (char.IsWhiteSpace(sb[sb.Length - 1]))
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb;
        }
    }
}