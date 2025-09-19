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

namespace JLIB3.JCalendar
{
    public static class JTimeStamp
    {
        /// <summary>
        /// Standard ISO format only
        /// </summary>
        public static string ISO
        {
            get { return "yyyy-MM-dd HH:mm:ss"; }
        }

        /// <summary>
        /// Returns ISO format with T aka ISO8601
        /// </summary>
        public static string ISOt
        {
            get { return "yyyy-MM-ddTHH:mm:ss"; }
        }

        public static string FullLog
        {
            get { return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); }
        }
        /// <summary>
        /// FullFile 
        /// </summary>
        public static string FullFile
        {
            get { return DateTime.Now.ToString("yyyyMMddHHmmss"); }
        }
        public static string Date
        {
            get { return DateTime.Now.ToString("yyyy-MM-dd"); }
        }
        public static string Month
        {
            get { return DateTime.Now.ToString("yyyy-MM"); }
        }

        public static List<DateTime> GetMondays(DateTime startTime, DateTime endTime)
        {

            //return output;
            return GetDaysOfWeek(startTime, endTime, DayOfWeek.Monday);
        }

        public static List<DateTime> GetDaysOfWeek(DateTime startTime, DateTime endTime, DayOfWeek dow)
        {
            var output = new List<DateTime>();

            var wkst = startTime.StartOfWeek(dow);

            if (wkst == startTime.Date)
            {
                output.Add(wkst);
            }

            wkst = wkst.AddDays(7);

            while (wkst < endTime)
            {
                output.Add(wkst);

                wkst = wkst.AddDays(7);
            }

            return output;
        }

        private static readonly Regex logTimeRegex = new Regex(
            @"(\d{4}-\d{2}-\d{2}[ T]?\d{2}:\d{2}(:\d{2})?)|(\d{4}-\w{3}-\d{2}[ T]?\d{2}:\d{2}(:\d{2})?)|(\d{2}-\d{2}-\d{4}[ T]?\d{2}:\d{2}(:\d{2})?)",
            RegexOptions.Compiled);

        public static string TranslateLogTimes(string input, TimeZoneInfo tz)
        {
            string[] insplt = input.Split('\n');

            for (int i = 0; i < insplt.Length; i++)
            {
                //string line = insplt[i];
                foreach (Match m in logTimeRegex.Matches(insplt[i]))
                {
                    var dt = m.Value.GetUTCDateTime();

                    if (dt == null)
                    {
                        continue;
                    }

                    var ndt = TimeZoneInfo.ConvertTime((DateTime)dt, tz);
                    insplt[i] = insplt[i].Replace(m.Value, ndt.ISOstring());
                }
            }

            return string.Join("\n", insplt);
        }

    }
}
