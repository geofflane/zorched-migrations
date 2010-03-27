using System.Data;
using NUnit.Framework;
using Rhino.Mocks;
using Zorched.Migrations.SqlServer.Schema;

namespace Zorched.Migrations.SqlServer.Tests.Operations
{

    [TestFixture]
    public class GenericOperationTests
    {

        private MockRepository mocks;
        private IDbCommand cmd;
        
        [SetUp]
        public void Setup()
        {
            mocks = new MockRepository();
            cmd = mocks.StrictMock<IDbCommand>();
        }

        [Test]
        public void should_be_able_to_execute_generic_operation()
        {
            var op = new SqlGenericOperation {Sql = "SELECT * FROM INFORMATION_SCHEMA.TABLES"};

            Expect.Call(() => cmd.CommandText = op.Sql);
            Expect.Call(cmd.ExecuteNonQuery()).Return(1);

            mocks.ReplayAll();
            op.Execute(cmd);
            Assert.AreEqual(op.Sql, op.ToString());
            
            mocks.VerifyAll();
        }
    }
}