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

            this.TopMost = true;
        }

        public BackgroundWorker ClockWorker { get; set; } = new BackgroundWorker();
        public List<WindowInfo> WinItems { get; private set; } = new List<WindowInfo>();

        public ImageList icons { get; private set; } = new ImageList();

        

        private void FormTaskBar_Load(object sender, EventArgs e)
        {
            RegisterAppBar();
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
                Win.ShowWindow(selectedWindow.Handle, Win.SW_RESTORE);
                Win.SetForegroundWindow(selectedWindow.Handle);
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

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left, top, right, bottom;
        }

        const int ABM_NEW = 0x00000000;
        const int ABM_REMOVE = 0x00000001;
        const int ABM_SETPOS = 0x00000003;
        const int ABE_LEFT = 0;

        private void RegisterAppBar()
        {
            APPBARDATA abd = new APPBARDATA();
            abd.cbSize = (uint)Marshal.SizeOf(abd);
            abd.hWnd = this.Handle;
            abd.uEdge = ABE_LEFT;

            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            abd.rc.top = screen.Top;
            abd.rc.bottom = screen.Bottom;
            abd.rc.left = screen.Left;
            abd.rc.right = screen.Left + this.Width;

            Win.SHAppBarMessage(ABM_NEW, ref abd);
            Win.SHAppBarMessage(ABM_SETPOS, ref abd);

            this.Location = new Point(abd.rc.left, abd.rc.top);
            this.Size = new Size(abd.rc.right - abd.rc.left, abd.rc.bottom - abd.rc.top);
        }


    }
}
