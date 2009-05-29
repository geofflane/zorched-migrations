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
using System.Configuration;
using System.Reflection;
using NUnit.Framework;
using Zorched.Migrations.Core;
using Zorched.Migrations.Framework;

namespace Zorched.Migrations.Tests.Framework
{
    [TestFixture]
    public class DriverLoaderTests
    {
        private readonly DriverLoader dl = new DriverLoader();

        [Test]
        public void can_get_assembly_by_name()
        {
            GetAssembly();
        }

        [Test]
        public void can_get_driver_from_assembly()
        {
            var cs = ConfigurationManager.ConnectionStrings["SqlServer"];
            var assembly = GetAssembly();
            var driver = dl.GetDriver(assembly, cs.ConnectionString, new TestLogger());
            Assert.IsNotNull(driver);
        }

        public Assembly GetAssembly()
        {
            var assembly = dl.GetAssemblyByName("Zorched.Migrations.Tests");
            Assert.IsNotNull(assembly);

            return assembly;
        }
    }

    public class TestLogger : ILogger
    {
        public void LogError(string error)
        {
            Console.Error.WriteLine(error);
        }

        public void LogError(string error, Exception ex)
        {
            Console.Error.WriteLine(error);
            Console.Error.WriteLine(ex.Message);
            Console.Error.WriteLine(ex.StackTrace);
        }

        public void LogInfo(string info)
        {
            Console.Out.WriteLine(info);
        }

        public void LogSql(string sql)
        {
            Console.Out.WriteLine(sql);
        }
    }
}