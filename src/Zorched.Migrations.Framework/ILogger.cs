using System;

namespace Zorched.Migrations.Framework
{
    public interface ILogger
    {

        void LogError(string error);

        void LogError(string error, Exception ex);

        void LogInfo(string info);

        void LogSql(string sql);
    }
}