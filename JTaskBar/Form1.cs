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
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace JTaskBar
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public BackgroundWorker ClockWorker { get; set; } = new BackgroundWorker();
        public List<WindowInfo> WinItems { get; private set; } = new List<WindowInfo>();

        private void Form1_Load(object sender, EventArgs e)
        {
            Timer_Clock.Start();
        }

        private void Timer_Clock_Tick(object sender, EventArgs e)
        {
            Lab_Clock.Text = DateTime.Now.ToString("HH:mm \ndddd\nyyyy-MM-dd") + "\n" + DateTime.Now.ISOWorkWeek();
            SetButtons();
        }

        private void SetButtons()
        {
            LiBx_Apps.Items.Clear();
            WinItems.Clear();

            foreach (var window in OpenWindowGetter.GetOpenWindows())
            {
                LiBx_Apps.Items.Add(window.Title);
                WinItems.Add(window);
            }
        }

        private void LiBx_Apps_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LiBx_Apps.SelectedIndex > -1)
            {
                var selectedWindow = WinItems[LiBx_Apps.SelectedIndex];
                ShowWindow(selectedWindow.Handle, SW_RESTORE);
                SetForegroundWindow(selectedWindow.Handle);
            }
        }

        private void LiBx_Apps_MouseMove(object sender, MouseEventArgs e)
        {
            ShowItemData(e.Location);
        }

        void ShowItemData(Point pt)
        {
            Point point = LiBx_Apps.PointToClient(Cursor.Position);
            int index = LiBx_Apps.IndexFromPoint(point);
            if (index <= 0) { return; }

            var info = WinItems[index];
            string tooltip = $"{info.Title}\n{info.ProcessName}\nParent: {info.ParentHandle}";
            TT_Win.Show(tooltip, this, new Point(pt.X + 8, pt.Y + 15), 2000);
        }

        private void Btn_Menu_Click(object sender, EventArgs e)
        {
            Menu_Main.Show();
        }

        private void Btn_Desktop_Click(object sender, EventArgs e)
        {
            // Placeholder for future minimize/restore all
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
            Close();
        }

        // Focus helpers
        [DllImport("user32.dll")] public static extern bool ShowWindowAsync(HandleRef hWnd, int nCmdShow);
        [DllImport("user32.dll")] public static extern bool SetForegroundWindow(IntPtr WindowHandle);
        [DllImport("User32.dll")] private static extern bool ShowWindow(IntPtr handle, int nCmdShow);
        [DllImport("User32.dll")] private static extern bool IsIconic(IntPtr handle);
        [DllImport("user32.dll")] static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
        [DllImport("user32.dll")] static extern uint GetCurrentThreadId();
        [DllImport("user32.dll")] static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);
        [DllImport("user32.dll")] static extern IntPtr GetForegroundWindow();

        const int SW_RESTORE = 9;

        public static void ForceFocus(IntPtr hWnd)
        {
            uint foreThread = GetWindowThreadProcessId(GetForegroundWindow(), out _);
            uint appThread = GetCurrentThreadId();

            if (foreThread != appThread)
            {
                AttachThreadInput(foreThread, appThread, true);
                SetForegroundWindow(hWnd);
                AttachThreadInput(foreThread, appThread, false);
            }
            else
            {
                SetForegroundWindow(hWnd);
            }

            ShowWindow(hWnd, SW_RESTORE);
        }
    }
}
