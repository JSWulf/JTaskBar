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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JLIB3.JCalendar
{
    public static class JDateTimeExt
    {
        public static DateTime GetDTFromString(string input)
        {

            return input.GetDTFromString();

        }

        /// <summary>
        /// Returns time in yyyy-MM-dd HH:mm:ss format
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ISOstring(this DateTime dt)
        {
            return dt.ToString(JTimeStamp.ISO);
        }

        // <summary>
        /// Returns time in yyyy-MM-dd HH:mm:ss format
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ISOtstring(this DateTime dt)
        {
            return dt.ToString(JTimeStamp.ISOt);
        }

        /// <summary>
        /// Returns time in yyyyMMddHHmmss format
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ISOFile(this DateTime dt)
        {
            return dt.ToString("yyyyMMddHHmmss");
        }

        /// <summary>
        /// Returns time in yyyy-MM-dd format
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ISOdate(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd");
        }

        

        public static string WorkWeekValue(this DateTime dt, DayOfWeek startDay)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(dt);
            if (day >= startDay && day <= (startDay + 2))
            {
                dt = dt.AddDays(3);
            }
            var wk = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(dt, CalendarWeekRule.FirstFourDayWeek, startDay);

            return CalWorkWeekYear(wk, dt);
        }

        private static string CalWorkWeekYear(int wk, DateTime dt)
        {
            string wks = (wk < 10) ? "0" + wk : wk.ToString();

            var y = dt.Year;

            if (wk == 1 && dt.Day > 20)
            {
                y++;
            }
            else if (wk > 51 && dt.Day < 10)
            {
                y = (dt.AddDays(-10)).Year;
            }

            return y + "/" + wks;
        }

        public static string ISOWorkWeek(this DateTime dt)
        {
            int wk = dt.GetIso8601WeekOfYear();
            string wks = (wk < 10) ? "0" + wk : wk.ToString();

            return CalWorkWeekYear(wk, dt);
        }

        public static string ISOWorkWeek(this DateTime? dtn)
        {
            return dtn?.ISOWorkWeek() ?? "";
        }

        public static int GetIso8601WeekOfYear(this DateTime time)
        {
            // Use Thursday/Friday/Saturday to get the week.
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        /// <summary>
        /// get the start of week on which ever day you specify 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="startOfWeek"></param>
        /// <returns></returns>
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        public static int Clamp(this int value, int min, int max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }

        public static DateTime NowIfNull(this DateTime? dt)
        {
            return (dt == null) ? DateTime.Now : (DateTime)dt;
        }

        /// <summary>
        /// Depreciated. Use SQL Library methods instead.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DBInput(this DateTime dt)
        {
            return "CONVERT(Varchar, '" + dt.ISOstring() + "', 120)";
        }
    }
}
