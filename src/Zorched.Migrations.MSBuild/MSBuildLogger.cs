using System;
using System.IO;
using Microsoft.Build.Utilities;
using ILogger=Zorched.Migrations.Framework.ILogger;

namespace Zorched.Migrations.MSBuild
{
    public class MSBuildLogger : ILogger, IDisposable
    {

        private StreamWriter writer;

        public MSBuildLogger(TaskLoggingHelper log, string scriptFile) : this(log)
        {
            if (! String.IsNullOrEmpty(scriptFile))
            {
                writer = new StreamWriter(scriptFile, true);
            }
        }

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
            LogSqlToFile(sql);
            Log.LogMessage(sql);
        }

        public void LogSqlToFile(string sql)
        {
            if (null != writer)
            {
                writer.WriteLine(sql);
            }
        }

        public void Dispose()
        {
            if (null != writer)
            {
                writer.Flush();
                writer.Dispose();
            }
        }
    }
}