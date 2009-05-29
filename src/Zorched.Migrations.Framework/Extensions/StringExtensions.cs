using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Zorched.Migrations.Framework.Extensions
{
    /// <summary>
    /// Extension methods for dealing with String types.
    /// </summary>
    public static class StringExtensions
    {
        static readonly Regex HUMANIZE_REGEX = new Regex("([A-Z])");

        /// <summary>
        /// Convert a classname to something more readable.
        /// ex.: CreateATable => Create a table
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public static string ToHumanName(this string className)
        {
            if (HUMANIZE_REGEX.IsMatch(className))
            {
                string humanName = HUMANIZE_REGEX.Replace(className, " $1");
                if (Char.IsUpper(className[0]))
                    humanName = humanName.Substring(1);     // remove the leading space we introduced
                return humanName.Substring(0, 1) + humanName.Substring(1).ToLower();
            }
            return className;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        /// <param name="placeholder"></param>
        /// <param name="replacement"></param>
        /// <returns></returns>
        public static string ReplaceOnce(this string template, string placeholder, string replacement)
        {
            int loc = template.IndexOf(placeholder);
            if (loc < 0)
            {
                return template;
            }
            else
            {
                return new StringBuilder(template.Substring(0, loc))
                    .Append(replacement)
                    .Append(template.Substring(loc + placeholder.Length))
                    .ToString();
            }
        }

        public static bool EndsWith(this StringBuilder sb, char val)
        {
            return sb[sb.Length - 1] == val;
        }

        public static StringBuilder TrimEnd(this StringBuilder sb, char val)
        {
            if (sb.EndsWith(val))
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb;
        }

        public static StringBuilder TrimEnd(this StringBuilder sb)
        {
            while (char.IsWhiteSpace(sb[sb.Length - 1]))
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb;
        }
    }
}