using System.Text;
using System.Text.RegularExpressions;

namespace Zorched.Migrations.Framework.Extensions
{
    /// <summary>
    /// Extension methods for dealing with String types.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Convert a classname to something more readable.
        /// ex.: CreateATable => Create a table
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public static string ToHumanName(this string className)
        {
            string name = Regex.Replace(className, "([A-Z])", " $1").Substring(1);
            return name.Substring(0, 1).ToUpper() + name.Substring(1).ToLower();
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