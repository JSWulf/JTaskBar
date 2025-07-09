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

                if (hWnd == shellWindow || !Win.IsWindowVisible(hWnd)) { return true; }

                //Exclude child windows
                if (Win.GetParent(hWnd) != IntPtr.Zero) { return true; }

                //Exclude tool windows
                int exStyle = Win.GetWindowLong(hWnd, Win.GWL_EXSTYLE);
                if ((exStyle & Win.WS_EX_TOOLWINDOW) != 0) { return true; }

                //Exclude windows without caption or system menu
                int style = Win.GetWindowLong(hWnd, Win.GWL_STYLE);
                bool hasCaption = (style & Win.WS_CAPTION) != 0;
                bool hasSysMenu = (style & Win.WS_SYSMENU) != 0;
                if (!hasCaption && !hasSysMenu) { return true; }


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
                    IconPath = null //Placeholder for future icon support
                });

                return true;
            }, 0);

            return windows
                .OrderBy(w => w.ProcessName.ToLowerInvariant())
                .ThenBy(w => w.Title.ToLowerInvariant())
                .ToList();

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


            if (Win.IsIconic(hWnd))
            {
                Win.ShowWindow(hWnd, Win.SW_RESTORE);
            }


        }

    }
}
