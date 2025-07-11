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
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using static JTaskBar.Win;

namespace JTaskBar
{
    public static class OpenWindowHandler //OpenWindowGetter
    {

        private static readonly HashSet<int> InaccessibleProcesses = new();


        public static List<WindowInfo> GetOpenWindows()
        {
            IntPtr shellWindow = GetShellWindow();
            List<WindowInfo> windows = new();

            EnumWindows(delegate (IntPtr hWnd, int lParam)
            {
                if (hWnd == shellWindow || !IsWindowVisible(hWnd)) { return true; }

                int length = GetWindowTextLength(hWnd);
                if (length == 0) { return true; }

                //Exclude child windows
                if (GetParent(hWnd) != IntPtr.Zero) { return true; }

                //Exclude tool windows
                int exStyle = Win.GetWindowLong(hWnd, Win.GWL_EXSTYLE);
                if ((exStyle & Win.WS_EX_TOOLWINDOW) != 0) { return true; }

                //Exclude windows without caption or system menu
                int style = GetWindowLong(hWnd, Win.GWL_STYLE);
                bool hasCaption = (style & Win.WS_CAPTION) != 0;
                bool hasSysMenu = (style & Win.WS_SYSMENU) != 0;
                if (!hasCaption && !hasSysMenu) { return true; }


                StringBuilder builder = new(length + 1);
                GetWindowText(hWnd, builder, builder.Capacity);

                GetWindowThreadProcessId(hWnd, out uint pid);
                string processName = string.Empty;
                try
                {
                    if (!InaccessibleProcesses.Contains((int)pid))
                    {
                        var process = Process.GetProcessById((int)pid);

                        string exePath = string.Empty;
                        string description = process.ProcessName;

                        //check if zombie window
                        //if (string.Equals(description, "ApplicationFrameHost", StringComparison.OrdinalIgnoreCase) ||
                        //    string.Equals(description, "ShellExperienceHost", StringComparison.OrdinalIgnoreCase) ||
                        //    string.Equals(description, "SystemSettings", StringComparison.OrdinalIgnoreCase))
                        //{
                            
                        //}

                        try
                        {
                            exePath = process.MainModule?.FileName ?? string.Empty;
                            if (!string.IsNullOrEmpty(exePath) && File.Exists(exePath))
                            {
                                description = FileVersionInfo.GetVersionInfo(exePath).FileDescription ?? process.ProcessName;
                            }
                        }
                        catch (Win32Exception)
                        {
                            InaccessibleProcesses.Add((int)pid); // Cache failure
                        }

                        processName = description;

                    }
                }
                catch
                {
                    InaccessibleProcesses.Add((int)pid); // Cache if process is gone or inaccessible
                }



                IntPtr parent = Win.GetParent(hWnd);

                windows.Add(new WindowInfo
                {
                    Handle = hWnd,
                    Title = builder.ToString(),
                    ProcessName = processName,
                    ParentHandle = parent,
                    //IconPath = null //Placeholder for future icon support
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

        /// <summary>
        /// Attempts to get the icon from the process, else from the executable
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        public static Icon GetWindowIcon(IntPtr hWnd)
        {
            IntPtr hIcon = SendMessage(hWnd, WM_GETICON, (IntPtr)ICON_SMALL, IntPtr.Zero);
            if (hIcon == IntPtr.Zero)
            {
                hIcon = GetClassLongPtr(hWnd, GCL_HICONSM);
            }

            if (hIcon != IntPtr.Zero)
            {
                return Icon.FromHandle(hIcon);
            }

            // Fallback: try to get icon from executable
            try
            {
                Win.GetWindowThreadProcessId(hWnd, out uint pid);
                var process = Process.GetProcessById((int)pid);
                string exePath = process.MainModule?.FileName;

                if (!string.IsNullOrEmpty(exePath) && File.Exists(exePath))
                {
                    return Icon.ExtractAssociatedIcon(exePath) ?? SystemIcons.Application;
                }
            }
            catch (Exception ex)
            {
                // Access denied or other issue
                Console.WriteLine($"Icon error: {ex.ToString()}");
            }

            return SystemIcons.Application;
        }

    }
}
