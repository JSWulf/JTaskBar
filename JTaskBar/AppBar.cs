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
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static JTaskBar.Win;

namespace JTaskBar
{
    public static class AppBar
    {
        public const int ABM_NEW = 0x00000000;
        public const int ABM_REMOVE = 0x00000001;
        public const int ABM_SETPOS = 0x00000003;
        public const int ABE_LEFT = 0;
        public const int ABE_RIGHT = 2;
        public const int WM_APPBARNOTIFY = 0x004B;
        public const int ABN_POSCHANGED = 0x0001;
        public const int WM_DISPLAYCHANGE = 0x007E;
        public const int WM_SETTINGCHANGE = 0x001A;

        private static bool isAppBarRegistered = false;

        public static uint callbackMessageId { get; private set; } = 0;


        private static bool isUpdatingPosition = false;

        public static void UpdateAppBarPosition(Form form, uint side, int width)
        {
            if (isUpdatingPosition)
            {
                return;
            }

            isUpdatingPosition = true;


            if (callbackMessageId == 0)
            {
                callbackMessageId = RegisterWindowMessage("AppBarMessage");
                Debug.WriteLine($"Registered AppBarMessage: {callbackMessageId}");
            }

            APPBARDATA abd = new APPBARDATA
            {
                cbSize = (uint)Marshal.SizeOf(typeof(APPBARDATA)),
                hWnd = form.Handle,
                uEdge = side,
                uCallbackMessage = (uint)callbackMessageId
            };

            if (!isAppBarRegistered)
            {
                Debug.WriteLine("Registering AppBar...");
                SHAppBarMessage(ABM_NEW, ref abd);
                isAppBarRegistered = true;
            }

            //Screen currentScreen = Screen.FromHandle(form.Handle);
            //Rectangle screen = Screen.FromHandle(form.Handle).WorkingArea;
            Rectangle screen = Screen.PrimaryScreen.Bounds;

            abd.rc.top = screen.Top;
            abd.rc.bottom = screen.Bottom;

            if (side == ABE_LEFT)
            {
                abd.rc.left = screen.Left;
                abd.rc.right = screen.Left + width;
            }
            else if (side == ABE_RIGHT)
            {
                abd.rc.right = screen.Right;
                abd.rc.left = screen.Right - width;
            }

            SHAppBarMessage(ABM_SETPOS, ref abd);

            form.Location = new Point(abd.rc.left, abd.rc.top);
            form.Size = new Size(abd.rc.right - abd.rc.left, abd.rc.bottom - abd.rc.top);

            //SetWindowPos(form.Handle, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);

            lastRedrawTime = DateTime.Now;
            Debug.WriteLine($"Position updated {lastRedrawTime}");

            isUpdatingPosition = false;
        }


        public static void UnsetAppBar(Form form)
        {
            APPBARDATA abd = new APPBARDATA
            {
                cbSize = (uint)Marshal.SizeOf(typeof(APPBARDATA)),
                hWnd = form.Handle
            };
            SHAppBarMessage(ABM_REMOVE, ref abd);
            isAppBarRegistered = false;
            Debug.WriteLine($"Position unset {DateTime.Now.ToString()}");
        }

        private static List<Rectangle> monitorBounds = new();
        private static Screen primaryScreen = Screen.PrimaryScreen;
        private static DateTime lastRedrawTime = DateTime.MinValue;
        private static readonly TimeSpan redrawCooldown = TimeSpan.FromMilliseconds(500);


        public static void CacheMonitorConfiguration()
        {
            monitorBounds = Screen.AllScreens.Select(s => s.Bounds).ToList();
            primaryScreen = Screen.PrimaryScreen;
        }

        private static bool AreRectsSignificantlyDifferent(Rectangle a, Rectangle b, int tolerance = 2)
        {
            return Math.Abs(a.Left - b.Left) > tolerance ||
                   Math.Abs(a.Top - b.Top) > tolerance ||
                   Math.Abs(a.Right - b.Right) > tolerance ||
                   Math.Abs(a.Bottom - b.Bottom) > tolerance;
        }

        public static bool HasMonitorConfigurationChanged()
        {
            //if ((DateTime.Now - lastRedrawTime) < redrawCooldown)
            //{
            //    return false;
            //}

            var currentBounds = Screen.AllScreens.Select(s => s.Bounds).ToList();
            var currentPrimary = Screen.PrimaryScreen;

            if (monitorBounds.Count != currentBounds.Count)
            {
                Debug.WriteLine("Monitor count changed.");
                return true;
            }

            for (int i = 0; i < monitorBounds.Count; i++)
            {
                if (AreRectsSignificantlyDifferent(monitorBounds[i], currentBounds[i]))
                {
                    Debug.WriteLine($"Monitor {i} bounds changed: {monitorBounds[i]} → {currentBounds[i]}");
                    return true;
                }
            }

            if (primaryScreen.DeviceName != currentPrimary.DeviceName)
            {
                Debug.WriteLine($"Primary screen changed: {primaryScreen.DeviceName} → {currentPrimary.DeviceName}");
                return true;
            }

            return false;
        }


        public static void LogMonitorConfiguration()
        {
            Debug.WriteLine("Current monitor configuration:");
            foreach (var screen in Screen.AllScreens)
            {
                Debug.WriteLine($"  Device: {screen.DeviceName}, Bounds: {screen.Bounds}, WorkingArea: {screen.WorkingArea}, Primary: {screen.Primary}");
            }
        }


    }

}
