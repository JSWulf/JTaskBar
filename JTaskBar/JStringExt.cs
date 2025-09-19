/*======================================================================================================================================================*\
|               .%#@+                                                                       *#%.                     ..    ..                     -*--@: |
|              -##+@.                                      +##.                             @##                    -###*  *#@                   -+:   *+ |
|             -###*                                      .##%*             +##%##+   -##*   =#=                   %#++%  :##%               -%=::.   .=+=|
|            :##:                                       :#@%+          -%#*   %##.  -##%   -##.                 .##.=%  .###.             .@%+@ -+. .. -+|
|       . -=###-                                       :###*         +##.    @##.  *##@.   @#-                 :#% #=   @##.             *@#=####:  ..=* |
|      %#=--##:                        **          :*--###-         .*:     %##. .@###.   %#.                 *#+*#:   +#%            +#@=+=%%@#%:*- *+  |
|         -##*           -@####=    -####*      :###%@##%                  %##. +#%##*   @#.  .%#+   .@#+.   -#@##.  .##%           =@+##@=:=#.  -@+.@.  |
|        +##*         .%###=  ##*.%#=.-##*   . @#=  *###.  :#.            @##.:##-*##. .#%  =####=  @###@. ..###+  .@##@  :@@.        %#@*.*=%#+--=+:@   |
|       =##@%%%%%+:-. --##-   @#-..  *#%  .%#:@#: -#*:#@ +#=             *##@##*  .##%##-    -##:.##.:##.:#%:#=   %#+##-@#*          =% +%++.+@:%@ :.@-  |
|  -+%@###:      .+###*.##  +#@.     @#=##@- -#@*#=   :@@:               %###-      .-       -####-  +###*  .#@*##::###+          .:##=:=%=#@.:@..%+=#+. |
|.%@=@##+            -.  *@=-         :+-     .+:                         ..                   ..     ..      ..   @#+#%          .*###+*%#:.##%*-=@*%:  |
|                   /\                                                                                            %#% :#*      .--=###-  ..#:.#+-%#-%#:  |
|                    \\ _____________________________________________________________________                    +#@ -#%    +#%. :..  *@@.  +=:*##:+@=-  |
|      (O)[\\\\\\\\\\(O)#####################################################################>                  -##..#+    . :@=-     .  :=-.+=+%**%#@   |
|                    //                                                                                         @#:+#.      .  .      ..-*+:.. -.*@.-..  |
|                   \/                                                                                         *##@-                ..      ..::.###:    |
\*======================================================================================================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Globalization;

namespace JLIB3
{
    public static class JStringExt
    {

        public static bool ContainsIgnoreCase(this String baseString, string Compare)
        {
            return (baseString != null && baseString.Contains(Compare, StringComparison.InvariantCultureIgnoreCase));
        }
        public static bool StartsWithIgnoreCase(this String baseString, string Compare)
        {
            return (!string.IsNullOrEmpty(baseString) && baseString.StartsWith(Compare, StringComparison.InvariantCultureIgnoreCase));
        }
        public static bool EqualsIgnoreCase(this String baseString, string Compare)
        {
            return (baseString != null && baseString.Equals(Compare, StringComparison.InvariantCultureIgnoreCase));
        }

        private static readonly List<(Regex Pattern, string Format)> TimePatterns = new List<(Regex Pattern, string Format)>()
        {
            // ISO 8601
            (new Regex(@"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}", RegexOptions.Compiled), "yyyy-MM-ddTHH:mm:ss"),
            (new Regex(@"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}", RegexOptions.Compiled), "yyyy-MM-ddTHH:mm"),

            // Compact formats
            (new Regex(@"^\d{14}", RegexOptions.Compiled), "yyyyMMddHHmmss"),
            (new Regex(@"^\d{13}", RegexOptions.Compiled), "yyyyMMddHmmss"),
            (new Regex(@"^\d{12}", RegexOptions.Compiled), "yyyyMMddHHmm"),
            (new Regex(@"^\d{11}", RegexOptions.Compiled), "yyyyMMddHmm"),

            (new Regex(@"^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}\.\d{3}", RegexOptions.Compiled), "yyyy-MM-dd HH:mm:ss.fff"),
            (new Regex(@"^\d{4}-\d{2}-\d{2}\|\d{2}:\d{2}:\d{2}\.\d{3}", RegexOptions.Compiled), "yyyy-MM-dd|HH:mm:ss.fff"),
            (new Regex(@"^\d{4}-\d{2}-\d{2} \d{1}:\d{2}:\d{2}\.\d{3}", RegexOptions.Compiled), "yyyy-MM-dd H:mm:ss.fff"),

            // yyyy-MM-dd HH:mm:ss variants
            (new Regex(@"^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}", RegexOptions.Compiled), "yyyy-MM-dd HH:mm:ss"),
            (new Regex(@"^\d{4}-\d{2}-\d{2} \d{1}:\d{2}:\d{2}", RegexOptions.Compiled), "yyyy-MM-dd H:mm:ss"),
            (new Regex(@"^\d{4}-\d{2}-\d{2} \d{2}:\d{2}", RegexOptions.Compiled), "yyyy-MM-dd HH:mm"),
            (new Regex(@"^\d{4}-\d{2}-\d{2} \d{1}:\d{2}", RegexOptions.Compiled), "yyyy-MM-dd H:mm"),

            // MM-dd-yyyy variants
            (new Regex(@"^\d{2}-\d{2}-\d{4} \d{2}:\d{2}:\d{2}", RegexOptions.Compiled), "MM-dd-yyyy HH:mm:ss"),
            (new Regex(@"^\d{2}-\d{2}-\d{4} \d{1}:\d{2}:\d{2}", RegexOptions.Compiled), "MM-dd-yyyy H:mm:ss"),
            (new Regex(@"^\d{2}-\d{2}-\d{4} \d{2}:\d{2}", RegexOptions.Compiled), "MM-dd-yyyy HH:mm"),
            (new Regex(@"^\d{2}-\d{2}-\d{4} \d{1}:\d{2}", RegexOptions.Compiled), "MM-dd-yyyy H:mm"),

            // MM/dd/yyyy variants
            (new Regex(@"^\d{2}/\d{2}/\d{4} \d{2}:\d{2}:\d{2}", RegexOptions.Compiled), "MM/dd/yyyy HH:mm:ss"),
            (new Regex(@"^\d{2}/\d{2}/\d{4} \d{1}:\d{2}:\d{2}", RegexOptions.Compiled), "MM/dd/yyyy H:mm:ss"),
            (new Regex(@"^\d{2}/\d{2}/\d{4} \d{2}:\d{2}", RegexOptions.Compiled), "MM/dd/yyyy HH:mm"),
            (new Regex(@"^\d{2}/\d{2}/\d{4} \d{1}:\d{2}", RegexOptions.Compiled), "MM/dd/yyyy H:mm"),

            // M/d/yyyy variants
            (new Regex(@"^\d{1}/\d{2}/\d{4} \d{2}:\d{2}:\d{2}", RegexOptions.Compiled), "M/dd/yyyy HH:mm:ss"),
            (new Regex(@"^\d{1}/\d{2}/\d{4} \d{1}:\d{2}:\d{2}", RegexOptions.Compiled), "M/dd/yyyy H:mm:ss"),
            (new Regex(@"^\d{1}/\d{2}/\d{4} \d{2}:\d{2}", RegexOptions.Compiled), "M/dd/yyyy HH:mm"),
            (new Regex(@"^\d{1}/\d{2}/\d{4} \d{1}:\d{2}", RegexOptions.Compiled), "M/dd/yyyy H:mm"),

            // MM/d/yyyy variants
            (new Regex(@"^\d{2}/\d{1}/\d{4} \d{2}:\d{2}:\d{2}", RegexOptions.Compiled), "MM/d/yyyy HH:mm:ss"),
            (new Regex(@"^\d{2}/\d{1}/\d{4} \d{1}:\d{2}:\d{2}", RegexOptions.Compiled), "MM/d/yyyy H:mm:ss"),
            (new Regex(@"^\d{2}/\d{1}/\d{4} \d{2}:\d{2}", RegexOptions.Compiled), "MM/d/yyyy HH:mm"),
            (new Regex(@"^\d{2}/\d{1}/\d{4} \d{1}:\d{2}", RegexOptions.Compiled), "MM/d/yyyy H:mm"),

            // M/d/yyyy variants
            (new Regex(@"^\d{1}/\d{1}/\d{4} \d{2}:\d{2}:\d{2}", RegexOptions.Compiled), "M/d/yyyy HH:mm:ss"),
            (new Regex(@"^\d{1}/\d{1}/\d{4} \d{1}:\d{2}:\d{2}", RegexOptions.Compiled), "M/d/yyyy H:mm:ss"),
            (new Regex(@"^\d{1}/\d{1}/\d{4} \d{2}:\d{2}", RegexOptions.Compiled), "M/d/yyyy HH:mm"),
            (new Regex(@"^\d{1}/\d{1}/\d{4} \d{1}:\d{2}", RegexOptions.Compiled), "M/d/yyyy H:mm"),

            // yyyy/MM/dd variants
            (new Regex(@"^\d{4}/\d{2}/\d{2} \d{2}:\d{2}:\d{2}", RegexOptions.Compiled), "yyyy/MM/dd HH:mm:ss"),
            (new Regex(@"^\d{4}/\d{2}/\d{2} \d{1}:\d{2}:\d{2}", RegexOptions.Compiled), "yyyy/MM/dd H:mm:ss"),
            (new Regex(@"^\d{4}/\d{2}/\d{2} \d{2}:\d{2}", RegexOptions.Compiled), "yyyy/MM/dd HH:mm"),
            (new Regex(@"^\d{4}/\d{2}/\d{2} \d{1}:\d{2}", RegexOptions.Compiled), "yyyy/MM/dd H:mm"),

            // yyyy-MM-dd|HH:mm:ss variants
            (new Regex(@"^\d{4}-\d{2}-\d{2}\|\d{2}:\d{2}:\d{2}", RegexOptions.Compiled), "yyyy-MM-dd|HH:mm:ss"),
            (new Regex(@"^\d{4}-\d{2}-\d{2}\|\d{1}:\d{2}:\d{2}", RegexOptions.Compiled), "yyyy-MM-dd|H:mm:ss"),
            (new Regex(@"^\d{4}-\d{2}-\d{2}\|\d{2}:\d{2}", RegexOptions.Compiled), "yyyy-MM-dd|HH:mm"),
            (new Regex(@"^\d{4}-\d{2}-\d{2}\|\d{1}:\d{2}", RegexOptions.Compiled), "yyyy-MM-dd|H:mm"),

            // dd-MMM-yyyy variants
            (new Regex(@"^\d{2}-[a-zA-Z]{3}-\d{4} \d{2}:\d{2}:\d{2}", RegexOptions.Compiled), "dd-MMM-yyyy HH:mm:ss"),
            (new Regex(@"^\d{2}-[a-zA-Z]{3}-\d{4} \d{1}:\d{2}:\d{2}", RegexOptions.Compiled), "dd-MMM-yyyy H:mm:ss"),
            (new Regex(@"^\d{2}-[a-zA-Z]{3}-\d{4} \d{2}:\d{2}", RegexOptions.Compiled), "dd-MMM-yyyy HH:mm"),
            (new Regex(@"^\d{2}-[a-zA-Z]{3}-\d{4} \d{1}:\d{2}", RegexOptions.Compiled), "dd-MMM-yyyy H:mm"),

            // dd-MMM-yyyy HHmmss variants
            (new Regex(@"^\d{2}-[a-zA-Z]{3}-\d{4} \d{2}\d{2}\d{2}", RegexOptions.Compiled), "dd-MMM-yyyy HHmmss"),
            (new Regex(@"^\d{2}-[a-zA-Z]{3}-\d{4} \d{1}\d{2}\d{2}", RegexOptions.Compiled), "dd-MMM-yyyy Hmmss"),
            (new Regex(@"^\d{2}-[a-zA-Z]{3}-\d{4} \d{2}\d{2}", RegexOptions.Compiled), "dd-MMM-yyyy HHmm"),
            (new Regex(@"^\d{2}-[a-zA-Z]{3}-\d{4} \d{1}\d{2}", RegexOptions.Compiled), "dd-MMM-yyyy Hmm"),

            // Date-only
            (new Regex(@"^\d{4}-\d{2}-\d{2}$", RegexOptions.Compiled), "yyyy-MM-dd"),
            (new Regex(@"^\d{2}-\d{2}-\d{4}$", RegexOptions.Compiled), "MM-dd-yyyy"),

            // Time-only
            (new Regex(@"^\d{2}:\d{2}:\d{2}$", RegexOptions.Compiled), "HH:mm:ss"),
            (new Regex(@"^\d{1}:\d{2}:\d{2}$", RegexOptions.Compiled), "H:mm:ss"),
            (new Regex(@"^\d{2}:\d{2}$", RegexOptions.Compiled), "HH:mm"),
            (new Regex(@"^\d{1}:\d{2}$", RegexOptions.Compiled), "H:mm"),

            // yyyyMMdd
            (new Regex(@"^\d{8}$", RegexOptions.Compiled), "yyyyMMdd"),
        };

        public static string? GetTimeFormat(this string baseString)
        {
            if (string.IsNullOrWhiteSpace(baseString))
                return null;

            foreach (var (pattern, format) in TimePatterns)
            {
                if (pattern.IsMatch(baseString))
                    return format;
            }

            return "";
        }


        /// <summary>
        /// Input the enum to check and return the matching value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T? ToEnum<T>(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return default(T);
            }
            return (T)Enum.Parse(typeof(T), value, true);
        }


        /// <summary>
        /// Returns UTC of converted datetime using RX
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime? GetUTCDateTime(this string value)
        {
            if (value == null)
            {
                return null;
            }
            var vt = value.Trim();
            var tf = vt.GetTimeFormat();

            if (string.IsNullOrEmpty(tf))
            {
                //return default(DateTime);
                return null;
            }
            else
            {
                var r = DateTime.TryParseExact(vt, tf, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result);

                return (r) ? result : null;
            }
        }

        /// <summary>
        /// Returns unassigned of converted datetime using RegEx
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime? GetDateTime(this string value)
        {
            if (value == null)
            {
                return null;
            }
            var vt = value.Trim();
            var tf = vt.GetTimeFormat();


            if (string.IsNullOrEmpty(tf))
            {
                //return default(DateTime);
                return null;
            }
            else
            {
                NormalizeDateTimeFormat(ref vt, ref tf);
                var r = DateTime.TryParseExact(vt, tf, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result);

                return (r) ? result : null;
            }
        }

        private static readonly string[] KnownFormatsTZ = new[]
        {
            "yyyy-MM-ddTHH:mm:ssZ",
            "yyyy-MM-ddTHH:mm:sszzz",
            "yyyy-MM-ddTHH:mm:ss.fffZ",
            "yyyy-MM-ddTHH:mm:ss.fffzzz",
            "yyyy-MM-dd HH:mm:ss zzz",
            "yyyy-MM-dd HH:mm:sszzz",
            "yyyy-MM-dd HH:mm:ss.fffzzz"
        };

        /// <summary>
        /// Returns unassigned of converted datetime
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime? GetDateTimeTZ(this string value)
        {
            if (value == null)
            {
                return null;
            }

            var vt = value.Trim();

            var r = DateTime.TryParseExact(
                    vt,
                    KnownFormatsTZ,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal,
                    out DateTime result);

            return (r)? result : GetDateTime(vt);
        }


        private static readonly HashSet<string> BadTimeFormats = new HashSet<string>
        {
            "yyyyMMddHmmss",
            "yyyyMMddHmm",
            "dd-MMM-yyyy Hmmss",
            "dd-MMM-yyyy Hmm",
            //"dd-MMM-yyyy H:mm:ss",
            //"dd-MMM-yyyy H:mm",
            //"dd-MMM-yyyy H:mm:ss.fff",
            "dd-MMM-yyyy Hmm",
            //"yyyy-MM-dd H:mm:ss",
            //"yyyy-MM-dd H:mm",
            //"MM-dd-yyyy H:mm:ss",
            //"MM-dd-yyyy H:mm",
            //"MM/dd/yyyy H:mm:ss",
            //"MM/dd/yyyy H:mm",
            //"yyyy/MM/dd H:mm:ss",
            //"yyyy/MM/dd H:mm",
            //"yyyy-MM-dd|H:mm:ss",
            //"yyyy-MM-dd|H:mm"
        };

        /// <summary>
        /// Normalize Bad Times
        /// </summary>
        /// <param name="input"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static bool NormalizeDateTimeFormat(ref string input, ref string format)
        {
            if (string.IsNullOrEmpty(format))
                return false;

            var originalFormat = format;

            // Normalize unsupported formats
            if (BadTimeFormats.Contains(originalFormat))
            {
                // Pad hour if needed (only for compact formats)
                if (originalFormat == "yyyyMMddHmmss" && input.Length == 13)
                {
                    input = input.Substring(0, 8) + "0" + input.Substring(8);
                }
                else if (originalFormat == "yyyyMMddHmm" && input.Length == 11)
                {
                    input = input.Substring(0, 8) + "0" + input.Substring(8);
                }

                // Now replace format tokens
                format = format.Replace("H:mm:ss", "HH:mm:ss")
                               .Replace("H:mm", "HH:mm")
                               .Replace("Hmmss", "HHmmss")
                               .Replace("Hmm", "HHmm");
            }

            // Normalize custom separator
            if (format.Contains('|'))
            {
                format = format.Replace("|", " ");
                input = input.Replace("|", " ");
            }

            return true;
        }





        public static string RemoveZeroWdith(this string input)
        {
            return input?
                .Replace("\u200B", "")   // zero-width space
                .Replace("\u200C", "")   // zero-width non-joiner
                .Replace("\u200D", "")   // zero-width joiner
                .Replace("\uFEFF", "")   // BOM / zero-width no-break space
                .Replace("\u00A0", " ")  // non-breaking space → regular space
                .Trim();
        }

        public static DateTime GetDTFromString(this string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                return DateTime.ParseExact(input,
                                input.GetTimeFormat(),//"yyyy-MM-dd HH:mm:ss",
                                System.Globalization.CultureInfo.InvariantCulture);

            }

            throw new Exception("No input string");

        }

        public static string UniqueWords(this string value, string val2)
        {
            var x = value.Split(' ').ToList();
            x.AddRange(val2.Split(' '));

            return $"{String.Join(" ", x.Distinct())}";
        }

        public static string UniqueWords(this List<string> li)
        {
            //join list of strings together with space in between
            string Concat = $"{String.Join(" ", li.Distinct())}";

            //split joined strings at spaces to get individual words
            var x = Concat.Split(' ').ToList();

            //return unique words
            return $"{String.Join(" ", x.Distinct())}";

        }

        

        public static StringIgnoreCase StringEqualsIgnoreCase { get; } = new StringIgnoreCase();

        /// <summary>
        /// Trims the string and returns null if the string is empty.
        /// </summary>
        /// <param name="str">String to Trim() and Check if Null</param>
        /// <returns></returns>
        public static string? NullIfEmpty(this String str)
        {
            var t = str.Trim();
            return (string.IsNullOrEmpty(t)) ? null : t;
        }

        public static string? DBSan(this String str)
        {
            return (str == null) ? null : str.Trim().Replace("'", "''");
        }


    }

    public class StringIgnoreCase : IEqualityComparer<string>
    {
        
        public bool Equals(string x, string y)
        {
            return x.EqualsIgnoreCase(y);
        }

        public int GetHashCode(string str)
        {
            int r = 0;
            foreach (char x in str)
            {
                r ^= x;
            }
            return r;
        }
    }
}
