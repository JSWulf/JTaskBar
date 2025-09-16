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
using static JTaskBar.AppBar;
using static JTaskBar.Win;


namespace JTaskBar
{
    public class WindowHooks
    {
        public WindowHooks(FormTaskBar form)
        {
            TaskBar = form;
        }

        public FormTaskBar TaskBar { get; private set; }

        private IntPtr hookHandle { get; set; } = IntPtr.Zero;

        private Win.WinEventDelegate? winEventProc { get; set; }

        public void StartWindowAlertHook()
        {
            winEventProc = new Win.WinEventDelegate(WinEventCallback);
            hookHandle = Win.SetWinEventHook(
                Win.EVENT_OBJECT_NAMECHANGE, Win.EVENT_OBJECT_NAMECHANGE,
                IntPtr.Zero, winEventProc, 0, 0, Win.WINEVENT_OUTOFCONTEXT);
        }

        public void StopWindowAlertHook()
        {
            if (hookHandle != IntPtr.Zero)
            {
                Win.UnhookWinEvent(hookHandle);
                hookHandle = IntPtr.Zero;
            }
        }

        private void WinEventCallback(IntPtr hWinEventHook, uint eventType, IntPtr hwnd,
                        int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            if (idObject == Win.OBJID_WINDOW && hwnd != IntPtr.Zero)
            {
                //uncomment if filtering alerts to specific screens
                //var screen = Screen.FromHandle(hwnd);
                //if (AssignedScreen != null && screen.DeviceName == AssignedScreen.DeviceName)
                //{
                NotifyWindowAlert(hwnd);
                //}
            }
        }

        public static bool IsWindowOnScreen(IntPtr hwnd, Screen screen)
        {
            return Screen.FromHandle(hwnd).DeviceName == screen.DeviceName;
        }

        public void NotifyWindowAlert(IntPtr hwnd)
        {
            var row = TaskBar.DGV_Apps.Rows.Cast<DataGridViewRow>()
                .FirstOrDefault(r => (r.DataBoundItem as WindowInfo)?.Handle == hwnd);
            if (row != null)
            {
                Task.Run(async () =>
                {
                    for (int i = 0; i < 20; i++)
                    {
                        TaskBar.Invoke(() => row.DefaultCellStyle.BackColor = (i % 2 == 0) ? Color.DarkRed : Color.Black);
                        await Task.Delay(250);
                    }
                    TaskBar.Invoke(() => row.DefaultCellStyle.BackColor = Color.DarkRed);
                });
            }
        }
    }
}
