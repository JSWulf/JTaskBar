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
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static JTaskBar.FormTaskBar;

namespace JTaskBar
{
    public static class Win
    {
        [DllImport("shell32.dll")] public static extern uint SHAppBarMessage(uint dwMessage, ref APPBARDATA pData);


        [DllImport("kernel32.dll")] public static extern uint GetCurrentThreadId();

        [DllImport("user32.dll")] public static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);
        [DllImport("user32.dll")] public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll")] public static extern int GetWindowTextLength(IntPtr hWnd);
        [DllImport("user32.dll")] public static extern bool IsWindowVisible(IntPtr hWnd);
        [DllImport("user32.dll")] public static extern IntPtr GetShellWindow();
        [DllImport("user32.dll")] public static extern IntPtr GetParent(IntPtr hWnd);
        [DllImport("user32.dll")] public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
        [DllImport("user32.dll")] public static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")] public static extern bool ShowWindowAsync(HandleRef hWnd, int nCmdShow);
        [DllImport("user32.dll")] public static extern bool SetForegroundWindow(IntPtr WindowHandle);
        [DllImport("User32.dll")] public static extern bool ShowWindow(IntPtr handle, int nCmdShow);
        [DllImport("User32.dll")] public static extern bool IsIconic(IntPtr handle);
        [DllImport("user32.dll")] public static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);
        [DllImport("user32.dll")] public static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")] public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", SetLastError = true)] public static extern IntPtr GetClassLongPtr(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")] public static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)] public static extern uint RegisterWindowMessage(string lpString);
        [DllImport("user32.dll")] public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        public delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);

        public const int SW_RESTORE = 9;
        public const int SW_MINIMIZE = 6;
        public const int GWL_STYLE = -16;
        public const int GWL_EXSTYLE = -20;
        public const int WS_EX_TOOLWINDOW = 0x00000080;
        public const int WS_CAPTION = 0x00C00000;
        public const int WS_SYSMENU = 0x00080000;
        public const int WM_GETICON = 0x007F;
        public const int ICON_SMALL = 0;
        public const int GCL_HICONSM = -34;
        public const uint WM_CLOSE = 0x0010;
        public const uint SWP_NOMOVE = 0x0002;
        public const uint SWP_NOSIZE = 0x0001;
        public const uint SWP_NOACTIVATE = 0x0010;

        public static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left, top, right, bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct APPBARDATA
        {
            public uint cbSize;
            public IntPtr hWnd;
            public uint uCallbackMessage;
            public uint uEdge;
            public RECT rc;
            public int lParam;
        }

    }
}
