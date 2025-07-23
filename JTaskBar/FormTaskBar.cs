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
using static JTaskBar.AppBar;
using static JTaskBar.Win;

namespace JTaskBar
{
    public partial class FormTaskBar : Form
    {
        public FormTaskBar()
        {
            InitializeComponent();

            TT_Win.InitialDelay = 2000;
            TT_Win.ReshowDelay = 500;
            TT_Win.AutoPopDelay = 5000;
            TT_Win.ShowAlways = false;

            icons.ImageSize = new Size(16, 16);
            LiVw_Apps.SmallImageList = icons;

            LiVw_Apps.Columns.Add("Window", this.Width - 18);//todo: set to dynamic value based on how wide the form is.

            this.TopMost = true;
            Debug.WriteLine("test starting...");
        }

        public BackgroundWorker ClockWorker { get; set; } = new BackgroundWorker();
        public List<WindowInfo> WinItems { get; private set; } = new List<WindowInfo>();

        public ImageList icons { get; private set; } = new ImageList();

        private int barWidth = 101;

        public int BarWidth
        {
            get { return barWidth; }
            set
            {
                barWidth = value;
                this.Width = value;
                LiVw_Apps.Columns[Window.Name].Width = this.Width - 18;
                UpdateAppBarPosition(this, DockSide, barWidth);
            }
        }

        private uint dockSide = ABE_LEFT;

        public uint DockSide
        {
            get { return dockSide; }
            set
            {
                dockSide = value;
                UpdateAppBarPosition(this, dockSide, BarWidth);
            }
        }

        private IntPtr lastFullscreenWindow = IntPtr.Zero;


        private void FormTaskBar_Load(object sender, EventArgs e)
        {
            //RegisterAppBar();
            UpdateAppBarPosition(this, DockSide, BarWidth);
            Timer_Clock.Start();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            //APPBARDATA abd = new APPBARDATA
            //{
            //    cbSize = (uint)Marshal.SizeOf(typeof(APPBARDATA)),
            //    hWnd = this.Handle
            //};
            //SHAppBarMessage(ABM_REMOVE, ref abd);

            UnsetAppBar(this);

            base.OnFormClosing(e);
        }


        private void Timer_Clock_Tick(object sender, EventArgs e)
        {
            Lab_Clock.Text = DateTime.Now.ToString("HH:mm \ndddd\nyyyy-MM-dd") + "\n" + DateTime.Now.ISOWorkWeek();
            SetButtons();


            // Fullscreen detection
            IntPtr foreground = Win.GetForegroundWindow();
            if (IsWindowFullscreen(foreground))
            {
                if (foreground != lastFullscreenWindow)
                {
                    lastFullscreenWindow = foreground;
                    OnEnterFullscreen(foreground);
                }
            }
            else if (lastFullscreenWindow != IntPtr.Zero)
            {
                OnExitFullscreen(lastFullscreenWindow);
                lastFullscreenWindow = IntPtr.Zero;
            }

        }

        /// <summary>
        /// Iterates through the window information and adds the items to the list.
        /// </summary>
        private void SetButtons()
        {
            var currentWindows = OpenWindowHandler.GetOpenWindows();
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
                    if (!icons.Images.ContainsKey(window.Handle.ToString()))
                    {
                        try
                        {
                            Icon icon = OpenWindowHandler.GetWindowIcon(window.Handle);
                            icons.Images.Add(window.Handle.ToString(), icon);
                        }
                        catch
                        {
                            icons.Images.Add(window.Handle.ToString(), SystemIcons.Application);
                        }
                    }

                    var item = new ListViewItem(window.Title)
                    {
                        ImageKey = window.Handle.ToString(),
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

            // Highlight the currently focused window
            IntPtr foreground = Win.GetForegroundWindow();

            foreach (ListViewItem item in LiVw_Apps.Items)
            {
                if (item.Tag is WindowInfo info && info.Handle == foreground)
                {
                    item.Selected = true;
                    item.EnsureVisible(); // Optional: scroll into view
                    break;
                }
            }

        }

        //store last click for minimize on click again.
        private IntPtr? lastFocusedHandle = null;

        private void LiVw_Apps_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LiVw_Apps.SelectedItems.Count == 0)
            {
                return;
            }

            var selectedItem = LiVw_Apps.SelectedItems[0];

            if (selectedItem.Tag is WindowInfo selectedWindow)
            {


                if (selectedWindow.Handle == Win.GetForegroundWindow())
                {
                    Win.ShowWindow(selectedWindow.Handle, Win.SW_MINIMIZE);
                }
                else
                {
                    OpenWindowHandler.ForceFocus(selectedWindow.Handle);
                    lastFocusedHandle = selectedWindow.Handle;

                    //if ForceFocus fails:
                    //Win.ShowWindow(selectedWindow.Handle, Win.SW_RESTORE);
                    //Win.SetForegroundWindow(selectedWindow.Handle);
                }
            }

        }

        private void LiVw_Apps_MouseDown(object sender, MouseEventArgs e)
        {
            var item = LiVw_Apps.GetItemAt(e.X, e.Y);
            if (item == null || item.Tag is not WindowInfo selectedWindow)
            {
                return;
            }

            if (e.Button == MouseButtons.Right)
            {
                //var item = LiVw_Apps.GetItemAt(e.X, e.Y);
                if (item != null)
                {
                    LiVw_Apps.SelectedItems.Clear();
                    item.Selected = true;
                    Menu_WindowBtn.Tag = item.Tag;
                    Menu_WindowBtn.Show(LiVw_Apps, e.Location);
                }
                return;
            }

            if (lastFocusedHandle.HasValue && selectedWindow.Handle == lastFocusedHandle.Value)
            {
                Win.ShowWindow(selectedWindow.Handle, Win.SW_MINIMIZE);
                lastFocusedHandle = IntPtr.Zero; // Reset to avoid double-minimize
            }
            else
            {
                OpenWindowHandler.ForceFocus(selectedWindow.Handle);
                lastFocusedHandle = selectedWindow.Handle;
            }

            LiVw_Apps.SelectedItems.Clear();
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
                string tooltip = $"{info.Title}\n{info.ProcessName}";//\nParent: {info.ParentHandle}

                Point screenPoint = LiVw_Apps.PointToScreen(new Point(e.X, e.Y));
                screenPoint.Offset(12, 24);
                TT_Win.Show(tooltip, this, screenPoint, 2000);

            }
        }


        /// <summary>
        /// Tooltip event on hover. Old method. Replaced with LiVw_Apps_MouseMove
        /// </summary>
        /// <param name="pt"></param>
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
                string tooltip = $"{info.ProcessName}\nParent: {info.ParentHandle}\n{info.Title}";
                TT_Win.Show(tooltip, this, new Point(pt.X + 8, pt.Y + 15), 2000);
            }
        }


        private void Btn_Menu_Click(object sender, EventArgs e)
        {
            Menu_Main.Show();
        }

        private List<IntPtr> minimizedWindows = new();

        /// <summary>
        /// Minimize all windows and restore on second click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Desktop_Click(object sender, EventArgs e)
        {
            if (minimizedWindows.Count == 0)
            {
                // Minimize all visible windows
                foreach (var win in WinItems)
                {
                    if (!Win.IsIconic(win.Handle))
                    {
                        Win.ShowWindow(win.Handle, Win.SW_MINIMIZE);
                        minimizedWindows.Add(win.Handle);
                    }
                }
            }
            else
            {
                // Restore previously minimized windows
                foreach (var hWnd in minimizedWindows)
                {
                    Win.ShowWindow(hWnd, Win.SW_RESTORE);
                }
                minimizedWindows.Clear();
            }
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
            Close();
            var x = this;
        }

        private void MBtn_Restore_Click(object sender, EventArgs e)
        {
            if (Menu_WindowBtn.Tag is WindowInfo info)
            {
                Win.ShowWindow(info.Handle, Win.SW_RESTORE);
                OpenWindowHandler.ForceFocus(info.Handle);
            }
        }

        private void MBtn_Minimize_Click(object sender, EventArgs e)
        {
            if (Menu_WindowBtn.Tag is WindowInfo info)
            {
                Win.ShowWindow(info.Handle, Win.SW_MINIMIZE);
            }
        }

        /// <summary>
        /// Opens file location for current process.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MBtn_Location_Click(object sender, EventArgs e)
        {
            if (Menu_WindowBtn.Tag is WindowInfo info)
            {
                try
                {
                    Win.GetWindowThreadProcessId(info.Handle, out uint pid);
                    var process = Process.GetProcessById((int)pid);
                    string path = process.MainModule?.FileName;

                    if (!string.IsNullOrEmpty(path))
                    {
                        Process.Start("explorer.exe", $"/select,\"{path}\"");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Unable to open file location: {ex.Message}");
                }
            }
        }


        private void MBtn_TskMn_Click(object sender, EventArgs e)
        {
            Process.Start("taskmgr.exe");
        }

        private void MBtn_Close_Click(object sender, EventArgs e)
        {
            if (Menu_WindowBtn.Tag is WindowInfo info)
            {
                PostMessage(info.Handle, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            }
        }

        /// <summary>
        /// Listener for screen changes - add a condition to redraw if true.
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            //Debug.WriteLine(m);

            if (m.Msg == WM_DISPLAYCHANGE || m.Msg == WM_SETTINGCHANGE)
            {
                if (HasMonitorConfigurationChanged())
                {
                    LogMonitorConfiguration();
                    Debug.WriteLine("Change positive");
                    //UnsetAppBar(this);
                    UpdateAppBarPosition(this, DockSide, BarWidth);
                    CacheMonitorConfiguration();
                }
            }
            //else if (m.Msg == WM_APPBARNOTIFY)
            //{
            //    Debug.WriteLine("Condition appbarnotify");
            //    int notifyCode = m.WParam.ToInt32();

            //    if (notifyCode == ABN_POSCHANGED)
            //    {
            //        Debug.WriteLine("ABN_POSCHANGED received");
            //        //UnsetAppBar(this);
            //        UpdateAppBarPosition(this, DockSide, BarWidth);
            //    }
            //}
            else if (m.Msg == callbackMessageId)
            {
                Debug.WriteLine($"AppBar callback received: {m.WParam}");
                int notifyCode = m.WParam.ToInt32();

                if (notifyCode == ABN_POSCHANGED)
                {
                    Debug.WriteLine("ABN_POSCHANGED received");
                    //UpdateAppBarPosition(this, DockSide, BarWidth);
                }
            }

        }



        private void reDrawToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //UnsetAppBar(this);

            UpdateAppBarPosition(this, DockSide, BarWidth);
        }

        private void OnEnterFullscreen(IntPtr hWnd)
        {
            Debug.WriteLine($"Entered fullscreen: {hWnd}");
            this.Visible = false;
            SetWindowPos(Handle, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);
        }

        private void OnExitFullscreen(IntPtr hWnd)
        {
            Debug.WriteLine($"Exited fullscreen: {hWnd}");
            this.Visible = true;
            SetWindowPos(Handle, IntPtr.Zero, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);
        }

    }
}
