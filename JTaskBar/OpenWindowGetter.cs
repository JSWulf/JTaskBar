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
    public static class OpenWindowGetter
    {
        public static List<WindowInfo> GetOpenWindows()
        {
            IntPtr shellWindow = Win.GetShellWindow();
            List<WindowInfo> windows = new();

            Win.EnumWindows(delegate (IntPtr hWnd, int lParam)
            {
                if (hWnd == shellWindow || !Win.IsWindowVisible(hWnd)) { return true; }

                int length = Win.GetWindowTextLength(hWnd);
                if (length == 0) { return true; }

                StringBuilder builder = new(length + 1);
                Win.GetWindowText(hWnd, builder, builder.Capacity);

                Win.GetWindowThreadProcessId(hWnd, out uint pid);
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

                IntPtr parent = Win.GetParent(hWnd);

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

        

        public static void ForceFocus(IntPtr hWnd)
        {
            uint foreThread = Win.GetWindowThreadProcessId(Win.GetForegroundWindow(), out _);
            uint appThread = Win.GetCurrentThreadId();

            if (foreThread != appThread)
            {
                Win.AttachThreadInput(foreThread, appThread, true);
                Win.SetForegroundWindow(hWnd);
                Win.AttachThreadInput(foreThread, appThread, false);
            }
            else
            {
                Win.SetForegroundWindow(hWnd);
            }

            Win.ShowWindow(hWnd, Win.SW_RESTORE);

        }

    }
}
