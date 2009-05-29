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

namespace Zorched.Migrations.Framework.Extensions
{
    /// <summary>
    /// Extension methods for dealing with IEnumerable types.
    /// </summary>
    public static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> input, Action<T> fn)
        {
            foreach (T item in input)
            {
                fn(item);
            }
        }

        public static void IterateOver<T>(this IEnumerable<T> input, Action<int, T> fn)
        {
            int i = 0;
            foreach (T item in input)
            {
                fn(i ++, item);
            }
        }

        public static IEnumerable<T2> CastAs<T1, T2>(this IEnumerable<T1> input, Func<T1, T2> fn)
        {
            var output = new List<T2>();
            input.ForEach(item => output.Add(fn(item)));
            return output;
        }
    }
}
