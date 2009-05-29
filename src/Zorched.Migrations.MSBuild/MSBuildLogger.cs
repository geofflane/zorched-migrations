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