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