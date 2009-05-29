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
using NUnit.Framework;
using Zorched.Migrations.Framework;
using Zorched.Migrations.SqlServer.Schema;

namespace Zorched.Migrations.SqlServer.Tests.Operations
{

    [TestFixture]
    public class DeleteUpdateHelperTests
    {

        private readonly DeleteUpdateHelper helper = new DeleteUpdateHelper();

        [Test]
        public void does_not_add_on_delete_if_not_the_property()
        {
            var sb = new StringBuilder();
            helper.AddOnDeleteIfNeeded(sb, ConstraintProperty.None);

            Assert.IsFalse(sb.ToString().Contains("ON DELETE"));
        }

        [Test]
        public void does_not_add_on_update_if_not_the_property()
        {
            var sb = new StringBuilder();
            helper.AddOnUpdateIfNeeded(sb, ConstraintProperty.None);

            Assert.IsFalse(sb.ToString().Contains("ON UPDATE"));
        }

        [Test]
        public void adds_on_delete_if_in_the_property()
        {
            var constraints = new[]
                                  {
                                      ConstraintProperty.CascadeOnDelete,
                                      ConstraintProperty.DefaultOnDelete,
                                      ConstraintProperty.NullOnDelete
                                  };
            Array.ForEach(constraints, c =>
                                           {
                                               var sb = new StringBuilder();
                                               helper.AddOnDeleteIfNeeded(sb, c);
                                               Assert.IsTrue(sb.ToString().Contains("ON DELETE"));
                                           });
        }


        [Test]
        public void adds_on_update_if_in_the_property()
        {
            var constraints = new[]
                                  {
                                      ConstraintProperty.CascadeOnUpdate,
                                      ConstraintProperty.DefaultOnUpdate,
                                      ConstraintProperty.NullOnUpdate
                                  };
            Array.ForEach(constraints, c =>
                                           {
                                               var sb = new StringBuilder();
                                               helper.AddOnUpdateIfNeeded(sb, c);
                                               Assert.IsTrue(sb.ToString().Contains("ON UPDATE"));
                                           });
        }
    }
}