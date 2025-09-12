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

            DGV_Apps.AutoGenerateColumns = false;

            TT_Win.InitialDelay = 2000;
            TT_Win.ReshowDelay = 500;
            TT_Win.AutoPopDelay = 5000;
            TT_Win.ShowAlways = false;

            icons.ImageSize = new Size(16, 16);

            DGVWins.DataSource = new BindingList<WindowInfo>();
            //DGVWins.DataSource = typeof(WindowInfo);

            DGV_Apps.DataSource = DGVWins;

            //DGV_Apps.AutoGenerateColumns = false;

            DGV_Apps.Columns.Add(new DataGridViewImageColumn
            {
                Name = "Icon",
                HeaderText = "",
                Width = 16,
                ImageLayout = DataGridViewImageCellLayout.Zoom,
                DataPropertyName = "IconIm"
            });
            //DGV_Apps.Columns.Add("Title", "Window Title");
            DGV_Apps.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Title",
                HeaderText = "Window Title",
                DataPropertyName = "Title",
                Width = this.Width// - 15
            });

            //DGV_Apps.Columns.Add("Process", "Process");
            DGV_Apps.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ProcessName",
                HeaderText = "Process",
                DataPropertyName = "ProcessName",
                Visible = false
            });

            DGV_Apps.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ProcessID",
                HeaderText = "Process ID",
                DataPropertyName = "ProcessID",
                Visible = false
            });

            //DGV_Apps.BackgroundColor = Color.Black;
            DGV_Apps.DefaultCellStyle.BackColor = Color.Black;
            DGV_Apps.DefaultCellStyle.ForeColor = Color.White;
            //DGV_Apps.DefaultCellStyle.SelectionBackColor = Color.DarkSlateGray;
            DGV_Apps.DefaultCellStyle.SelectionForeColor = Color.White;
            DGV_Apps.GridColor = Color.Black;
            DGV_Apps.EnableHeadersVisualStyles = false;


            this.TopMost = true;
            Debug.WriteLine("test starting...");

            DGV_Apps.CellClick += DGV_Apps_CellClick;
            DGV_Apps.CellMouseDown += DGV_Apps_MouseDown;
            DGV_Apps.MouseMove += DGV_Apps_MouseMove;
        }

        public BindingSource DGVWins { get; set; } = new BindingSource();
        //public BackgroundWorker ClockWorker { get; set; } = new BackgroundWorker();
        //public List<WindowInfo> WinItems { get; private set; } = new List<WindowInfo>();

        public ImageList icons { get; private set; } = new ImageList();

        //public static FormTaskBar? Instance { get; private set; }
        public Screen AssignedScreen { get; set; }

        private int barWidth = 101;

        public int BarWidth
        {
            get { return barWidth; }
            set
            {
                barWidth = value;
                this.Width = value;
                //LiVw_Apps.Columns[Window.Name].Width = this.Width - 18;
                DGV_Apps.Columns["Title"].Width = this.Width - 20;
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

        private IntPtr hookHandle = IntPtr.Zero;

        private Win.WinEventDelegate? winEventProc;

        private ToolStripDropDown? calendarPopup;

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


        private void FormTaskBar_Load(object sender, EventArgs e)
        {
            //RegisterAppBar();
            UpdateAppBarPosition(this, DockSide, BarWidth);
            Timer_Clock.Start();
            StartWindowAlertHook();

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            StopWindowAlertHook();

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

        private void DGV_Apps_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            { return; }
            var row = DGV_Apps.Rows[e.RowIndex];
            if (row.DataBoundItem is not WindowInfo selectedWindow)
            { return; }

            if (lastFocusedHandle.HasValue && selectedWindow.Handle == lastFocusedHandle.Value)
            {
                Win.ShowWindow(selectedWindow.Handle, Win.SW_MINIMIZE);
                lastFocusedHandle = IntPtr.Zero;
            }
            else
            {
                OpenWindowHandler.ForceFocus(selectedWindow.Handle);
                lastFocusedHandle = selectedWindow.Handle;
            }
        }

        private void DGV_Apps_MouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                DGV_Apps.ClearSelection();
                DGV_Apps.Rows[e.RowIndex].Selected = true;

                var row = DGV_Apps.Rows[e.RowIndex];
                if (row.DataBoundItem is WindowInfo info)
                {
                    Menu_WindowBtn.Tag = info;

                    Rectangle cellRect = DGV_Apps.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                    Point screenPoint = DGV_Apps.PointToScreen(new Point(cellRect.X, cellRect.Y));

                    Menu_WindowBtn.Show(screenPoint);
                }
            }
        }

        private DataGridViewRow? lastTooltipRow = null;

        private void DGV_Apps_MouseMove(object sender, MouseEventArgs e)
        {
            var hit = DGV_Apps.HitTest(e.X, e.Y);
            if (hit.RowIndex >= 0)
            {
                var row = DGV_Apps.Rows[hit.RowIndex];
                if (row == lastTooltipRow)
                { return; }
                lastTooltipRow = row;

                if (row.Tag is WindowInfo info)
                {
                    string tooltip = $"{info.Title}\n{info.ProcessName}";
                    Point screenPoint = DGV_Apps.PointToScreen(new Point(e.X, e.Y));
                    screenPoint.Offset(12, 24);
                    TT_Win.Show(tooltip, DGV_Apps, DGV_Apps.PointToClient(screenPoint), 2000);
                }
            }
            else
            {
                lastTooltipRow = null;
                TT_Win.Hide(DGV_Apps);
            }
        }


        /// <summary>
        /// Iterates through the window information and adds the items to the list.
        /// </summary>
        private void SetButtons()
        {

            var currentWindows = OpenWindowHandler.GetOpenWindows();
            //var existingHandles = new HashSet<IntPtr>(WinItems.Select(w => w.Handle));
            var newHandles = new HashSet<IntPtr>(currentWindows.Select(w => w.Handle));

            //var windowList = (List<WindowInfo>)DGVWins.DataSource;
            var windowList = DGVWins.List as BindingList<WindowInfo>;

            int added = 0;
            // Update or add new items
            foreach (var window in currentWindows)
            {

                var existingItem = windowList.FirstOrDefault(w => w.Handle == window.Handle);

                if (existingItem != null)
                {
                    if (existingItem.Title != window.Title || existingItem.ProcessName != window.ProcessName)
                    {
                        existingItem.Title = window.Title;
                        existingItem.ProcessName = window.ProcessName;
                    }
                }
                else
                {
                    windowList.Add(window);
                    added++;
                }
            }

            // Remove closed windows
            for (int i = windowList.Count - 1; i >= 0; i--)
            {
                if (!newHandles.Contains(windowList[i].Handle))
                {
                    windowList.RemoveAt(i);
                }
            }

            if (added > 0)
            {
                //windowList.OrderBy(x => x.ProcessName).ThenBy(y => y.ProcessID).ToList();


                var sorted = windowList.OrderBy(x => x.ProcessName).ThenBy(x => x.ProcessID).ToList();

                windowList.RaiseListChangedEvents = false;
                windowList.Clear();
                foreach (var item in sorted)
                    windowList.Add(item);
                windowList.RaiseListChangedEvents = true;
                windowList.ResetBindings();

            }

            // Highlight the currently focused window
            IntPtr foreground = Win.GetForegroundWindow();
            var focused = windowList.FirstOrDefault(w => w.Handle == foreground);
            if (focused != null)
            {
                int index = windowList.IndexOf(focused);
                DGV_Apps.Rows[index].Selected = true;
                DGV_Apps.FirstDisplayedScrollingRowIndex = index;
            }

        }

        //store last click for minimize on click again.
        private IntPtr? lastFocusedHandle { get; set; } = null;

       
        private void Btn_Menu_Click(object sender, EventArgs e)
        {


            Point buttonScreenPoint = Btn_Menu.PointToScreen(Point.Empty);
            buttonScreenPoint.Offset(0, Btn_Menu.Height);

            if (dockSide == ABE_RIGHT)
            {
                buttonScreenPoint.Offset(-Btn_Menu.Width, 0);
            }

            Menu_Main.Show(buttonScreenPoint);

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
                var WinItems = DGVWins.List as BindingList<WindowInfo>;
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
            //this caused BSOD when restoring a large number of windows. - turned off for now.
            //else
            //{
            //    // Restore previously minimized windows
            //    foreach (var hWnd in minimizedWindows)
            //    {
            //        Win.ShowWindow(hWnd, Win.SW_RESTORE);
            //    }
            //    minimizedWindows.Clear();
            //}
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

        public void NotifyWindowAlert(IntPtr hwnd)
        {
            var row = DGV_Apps.Rows.Cast<DataGridViewRow>()
                .FirstOrDefault(r => (r.DataBoundItem as WindowInfo)?.Handle == hwnd);
            if (row != null)
            {
                Task.Run(async () =>
                {
                    for (int i = 0; i < 6; i++)
                    {
                        Invoke(() => row.DefaultCellStyle.BackColor = (i % 2 == 0) ? Color.DarkRed : Color.Black);
                        await Task.Delay(250);
                    }
                    Invoke(() => row.DefaultCellStyle.BackColor = Color.Black);
                });
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

        private void Menu_LRToggle_Click(object sender, EventArgs e)
        {
            DockSide = (DockSide == ABE_LEFT) ? (uint)ABE_RIGHT : (uint)ABE_LEFT;
            
        }

        private void Lab_Clock_Click(object sender, EventArgs e)
        {
            if (calendarPopup != null && calendarPopup.Visible)
            {
                calendarPopup.Close();
                calendarPopup = null;
                return;
            }

            MonthCalendar calendar = new MonthCalendar
            {
                MaxSelectionCount = 1,
                ShowToday = true,
                ShowTodayCircle = true
            };

            ToolStripControlHost host = new ToolStripControlHost(calendar)
            {
                Margin = Padding.Empty,
                Padding = Padding.Empty,
                AutoSize = false,
                Size = calendar.Size
            };

            calendarPopup = new ToolStripDropDown
            {
                Padding = Padding.Empty
            };
            calendarPopup.Items.Add(host);

            Point screenPoint = Lab_Clock.PointToScreen(Point.Empty);
            screenPoint.Offset(0, Lab_Clock.Height);
            calendarPopup.Show(screenPoint);
        }

    }
}
