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
    public partial class FormTaskBar : Form
    {
        public FormTaskBar()
        {
            InitializeComponent();

            icons.ImageSize = new Size(16, 16);
            LiVw_Apps.SmallImageList = icons;

            LiVw_Apps.Columns.Add("Window", 200);//todo: set to dynamic value based on how wide the form is.

        }

        public BackgroundWorker ClockWorker { get; set; } = new BackgroundWorker();
        public List<WindowInfo> WinItems { get; private set; } = new List<WindowInfo>();

        public ImageList icons { get; private set; } = new ImageList();

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
            var currentWindows = OpenWindowGetter.GetOpenWindows();
            var existingHandles = new HashSet<IntPtr>(WinItems.Select(w => w.Handle));
            var newHandles = new HashSet<IntPtr>(currentWindows.Select(w => w.Handle));

            // Update or add new items
            foreach (var window in currentWindows)
            {
                var existingItem = LiVw_Apps.Items
                    .Cast<ListViewItem>()
                    .FirstOrDefault(i => i.Tag is WindowInfo info && info.Handle == window.Handle);

                if (existingItem != null)
                {
                    // Update if title or process name changed
#pragma warning disable CS8600, CS8602
                    WindowInfo existingInfo = (WindowInfo)existingItem.Tag;

                    if (existingInfo.Title != window.Title || existingInfo.ProcessName != window.ProcessName)
                    {
                        existingItem.Text = window.Title;
                        existingItem.Tag = window;
                    }
#pragma warning restore CS8600, CS8602
                }
                else
                {
                    var item = new ListViewItem(window.Title)
                    {
                        //ImageIndex = iconIndex,
                        Tag = window
                    };
                    LiVw_Apps.Items.Add(item);
                }
            }

            // Remove closed windows
            for (int i = LiVw_Apps.Items.Count - 1; i >= 0; i--)
            {
                var item = LiVw_Apps.Items[i];
                if (item.Tag is WindowInfo info && !newHandles.Contains(info.Handle))
                {
                    LiVw_Apps.Items.RemoveAt(i);
                }
            }

            // Update WinItems cache
            WinItems = currentWindows;
        }



        private void LiVw_Apps_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LiVw_Apps.SelectedItems.Count == 0)
            {
                return;
            }

            var selectedItem = LiVw_Apps.SelectedItems[0];

            if (selectedItem.Tag is WindowInfo selectedWindow)
            {
                ShowWindow(selectedWindow.Handle, SW_RESTORE);
                SetForegroundWindow(selectedWindow.Handle);
            }
        }


        private ListViewItem? lastTooltipItem = null;

        private void LiVw_Apps_MouseMove(object sender, MouseEventArgs e)
        {
            var item = LiVw_Apps.GetItemAt(e.X, e.Y);

            if (item == null || item == lastTooltipItem)
            {
                return;
            }

            lastTooltipItem = item;

            if (item.Tag is WindowInfo info)
            {
                string tooltip = $"{info.Title}\n{info.ProcessName}\nParent: {info.ParentHandle}";
                TT_Win.Show(tooltip, this, new Point(e.X + 8, e.Y + 15), 2000);
            }
        }


        private void ShowItemData(Point pt)
        {
            Point point = LiVw_Apps.PointToClient(Cursor.Position);
            ListViewItem? item = LiVw_Apps.GetItemAt(point.X, point.Y);

            if (item == null)
            {
                return;
            }

            if (item.Tag is WindowInfo info)
            {
                string tooltip = $"{info.Title}\n{info.ProcessName}\nParent: {info.ParentHandle}";
                TT_Win.Show(tooltip, this, new Point(pt.X + 8, pt.Y + 15), 2000);
            }
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
