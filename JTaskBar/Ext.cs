/*==================================================================================================================================================*\
|               .%#@+                                                                       *#%.                     ..    ..                 .*- *. |
|              -##+@.                                      +##.                             @##                    -###*  *#@              -**-  .+* |
|             -###*                                      .##%*             +##%##+   -##*   =#=                   %#++%  :##%            .#%=-=.  .* |
|            :##:                                       :#@%+          -%#*   %##.  -##%   -##.                 .##.=%  .###.          :@#=%##*..-*  |
|       . -=###-                                       :###*         +##.    @##.  *##@.   @#-                 :#% #=   @##.         *%%#=:@+-:%:+.  |
|      %#=--##:                        **          :*--###-         .*:     %##. .@###.   %#.                 *#+*#:   +#%            +@+*:+@*@:*%-  |
|         -##*           -@####=    -####*      :###%@##%                  %##. +#%##*   @#.  .%#+   .@#+.   -#@##.  .##%          ..=#+*=#+-+.=*#*  |
|        +##*         .%###=  ##*.%#=.-##*   . @#=  *###.  :#.            @##.:##-*##. .#%  =####=  @###@. ..###+  .@##@  :@@.      .##*:%%-##@=@@:  |
|       =##@%%%%%+:-. --##-   @#-..  *#%  .%#:@#: -#*:#@ +#=             *##@##*  .##%##-    -##:.##.:##.:#%:#=   %#+##-@#*    .==:+*..+* -=*+=+%=:  |
|  -+%@###:      .+###*.##  +#@.     @#=##@- -#@*#=   :@@:               %###-      .-       -####-  +###*  .#@*##::###+       .:=:.  . .*--+=%*@#   |
|.%@=@##+            -.  *@=-         :+-     .+:                         ..                   ..     ..      ..   @#+#%         ..   .   --.-+#%.   |
|                   /\                                                                                            %#% :#*                            |
|                    \\ _____________________________________________________________________                    +#@ -#%                             |
|      (O)[\\\\\\\\\\(O)#####################################################################>                  -##..#+                              |
|                    //                                                                                         @#:+#.                               |
|                   \/                                                                                         *##@-                                 |
\*==================================================================================================================================================*/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace JTaskBar
{
    public static class Ext
    {
        public static string ISOWorkWeek(this DateTime dt)
        {
            int wk = dt.GetIso8601WeekOfYear();
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
            return $"{y}/{wks}";
        }

        public static int GetIso8601WeekOfYear(this DateTime time)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            { 
                time = time.AddDays(3); 
            }
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
                time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
    }

    public static class OpenWindowGetter
    {
        public static List<WindowInfo> GetOpenWindows()
        {
            IntPtr shellWindow = GetShellWindow();
            List<WindowInfo> windows = new();

            EnumWindows(delegate (IntPtr hWnd, int lParam)
            {
                if (hWnd == shellWindow || !IsWindowVisible(hWnd)) { return true; }

                int length = GetWindowTextLength(hWnd);
                if (length == 0) { return true; }

                StringBuilder builder = new(length + 1);
                GetWindowText(hWnd, builder, builder.Capacity);

                GetWindowThreadProcessId(hWnd, out uint pid);
                string processName = string.Empty;
                try
                {
                    processName = Process.GetProcessById((int)pid).ProcessName;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    //debug log here.
                }

                IntPtr parent = GetParent(hWnd);

                windows.Add(new WindowInfo
                {
                    Handle = hWnd,
                    Title = builder.ToString(),
                    ProcessName = processName,
                    ParentHandle = parent,
                    IconPath = null // Placeholder for future icon support
                });

                return true;
            }, 0);

            return windows;
        }

        private delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);

        [DllImport("user32.dll")] private static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);
        [DllImport("user32.dll")] private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll")] private static extern int GetWindowTextLength(IntPtr hWnd);
        [DllImport("user32.dll")] private static extern bool IsWindowVisible(IntPtr hWnd);
        [DllImport("user32.dll")] private static extern IntPtr GetShellWindow();
        [DllImport("user32.dll")] private static extern IntPtr GetParent(IntPtr hWnd);
        [DllImport("user32.dll")] private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
    }
}
