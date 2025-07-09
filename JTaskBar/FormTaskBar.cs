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
        }

        public BackgroundWorker ClockWorker { get; set; } = new BackgroundWorker();
        public List<WindowInfo> WinItems { get; private set; } = new List<WindowInfo>();

        public ImageList icons { get; private set; } = new ImageList();

        private int barWidth = 101;

        public int BarWidth
        {
            get { return barWidth; }
            set { 
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
            set { 
                dockSide = value;
                UpdateAppBarPosition(this, dockSide, BarWidth);
            }
        }

        private void FormTaskBar_Load(object sender, EventArgs e)
        {
            //RegisterAppBar();
            UpdateAppBarPosition(this, DockSide, BarWidth);
            Timer_Clock.Start();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            APPBARDATA abd = new APPBARDATA
            {
                cbSize = (uint)Marshal.SizeOf(typeof(APPBARDATA)),
                hWnd = this.Handle
            };
            Win.SHAppBarMessage(ABM_REMOVE, ref abd);

            base.OnFormClosing(e);
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
                    OpenWindowGetter.ForceFocus(selectedWindow.Handle);
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

            if (lastFocusedHandle.HasValue && selectedWindow.Handle == lastFocusedHandle.Value)
            {
                Win.ShowWindow(selectedWindow.Handle, Win.SW_MINIMIZE);
                lastFocusedHandle = IntPtr.Zero; // Reset to avoid double-minimize
            }
            else
            {
                OpenWindowGetter.ForceFocus(selectedWindow.Handle);
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
                
                //TT_Win.Show(tooltip, this, new Point(e.X + 8, e.Y + 15), 2000);

                Point screenPoint = LiVw_Apps.PointToScreen(new Point(e.X, e.Y));
                screenPoint.Offset(12, 24);
                TT_Win.Show(tooltip, this, screenPoint, 5000);

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
                string tooltip = $"{info.ProcessName}\nParent: {info.ParentHandle}\n{info.Title}";
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
            var x = this;
        }

        

        

        

        //private void RegisterAppBar()
        //{
        //    APPBARDATA abd = new APPBARDATA();
        //    abd.cbSize = (uint)Marshal.SizeOf(abd);
        //    abd.hWnd = this.Handle;
        //    abd.uEdge = DockSide;

        //    Rectangle screen = Screen.PrimaryScreen.WorkingArea;
        //    abd.rc.top = screen.Top;
        //    abd.rc.bottom = screen.Bottom;
        //    abd.rc.left = screen.Left;
        //    abd.rc.right = screen.Left + this.Width;

        //    Win.SHAppBarMessage(ABM_NEW, ref abd);
        //    Win.SHAppBarMessage(ABM_SETPOS, ref abd);

        //    this.Location = new Point(abd.rc.left, abd.rc.top);
        //    this.Size = new Size(abd.rc.right - abd.rc.left, abd.rc.bottom - abd.rc.top);
        //}

        



    }
}
