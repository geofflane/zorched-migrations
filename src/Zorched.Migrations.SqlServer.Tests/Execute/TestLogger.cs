using System;
using Zorched.Migrations.Framework;

namespace Zorched.Migrations.SqlServer.Tests.Execute
{
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