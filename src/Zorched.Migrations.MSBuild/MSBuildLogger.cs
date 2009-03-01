using System;
using Microsoft.Build.Utilities;
using ILogger=Zorched.Migrations.Framework.ILogger;

namespace Zorched.Migrations.MSBuild
{
    public class MSBuildLogger : ILogger
    {

        public MSBuildLogger(TaskLoggingHelper log)
        {
            Log = log;
        }

        public TaskLoggingHelper Log { get; set; }

        public void LogError(string error)
        {
            Log.LogError(error);
        }

        public void LogError(string error, Exception ex)
        {
            Log.LogErrorFromException(ex, true);
        }

        public void LogInfo(string info)
        {
            Log.LogMessage(info);
        }

        public void LogSql(string sql)
        {
            Log.LogMessage(sql);
        }
    }
}