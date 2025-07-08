using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using HWND = System.IntPtr;


namespace JTaskBar
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Timer_Clock.Start();
            //ClockWorker.WorkerReportsProgress = true;
        }

        public BackgroundWorker ClockWorker { get; set; } = new BackgroundWorker();
        public List<KeyValuePair<IntPtr, string>> WinItems { get; private set; } = new List<KeyValuePair<HWND, string>>();

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
            Close();
        }

        private void Btn_Menu_Click(object sender, EventArgs e)
        {
            Menu_Main.Show();
        }

        private void Btn_Desktop_Click(object sender, EventArgs e)
        {

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

            foreach (KeyValuePair<IntPtr, string> window in OpenWindowGetter.GetOpenWindows())
            {
                IntPtr handle = window.Key;
                string title = window.Value;

                //Console.WriteLine("{0}: {1}", handle, title);
                LiBx_Apps.Items.Add(title);
                WinItems.Add(window);
            }
        }

        private void LiBx_Apps_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LiBx_Apps.SelectedIndex > -1)
            {
                //FocusProcess(LiBx_Apps.SelectedItem.ToString());
                
                ShowWindow(WinItems[LiBx_Apps.SelectedIndex].Key, SW_RESTORE);
                SetForegroundWindow(WinItems[LiBx_Apps.SelectedIndex].Key);

            }
        }

        private void LiBx_Apps_MouseMove(object sender, MouseEventArgs e)
        {
            ShowItemData(e.Location);
        }

        ////ToolTip tt = new ToolTip();

        void ShowItemData(Point pt)
        {

            Point point = LiBx_Apps.PointToClient(Cursor.Position);
            int index = LiBx_Apps.IndexFromPoint(point);
            if (index <= 0) return;

            //Do your thing with the item:
            TT_Win.Show(LiBx_Apps.Items[index].ToString(), this, new Point(pt.X + 8, pt.Y + 15), 2000);
            //Console.WriteLine(LiBx_Apps.Items[index].ToString());

        }

        [DllImport("user32.dll")]
        public static extern bool ShowWindowAsync(HandleRef hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr WindowHandle);
        const int SW_RESTORE = 9;
        [DllImport("User32.dll")]
        private static extern bool ShowWindow(IntPtr handle, int nCmdShow);
        [DllImport("User32.dll")]
        private static extern bool IsIconic(IntPtr handle);

        //private void FocusProcess(IntPtr procID)
        //{
        //Process objProcesses = System.Diagnostics.Process.GetProcessById((int)procID);
        //if (objProcesses != null)
        //{
        //    IntPtr hWnd = IntPtr.Zero;
        //    hWnd = objProcesses.MainWindowHandle;
        //    ShowWindowAsync(new HandleRef(null, hWnd), SW_RESTORE);
        //    SetForegroundWindow(objProcesses.MainWindowHandle);
        //}
        //}
        private void FocusProcess(string procName)
        {
            Process[] objProcesses = System.Diagnostics.Process.GetProcessesByName(procName);
            if (objProcesses.Length > 0)
            {
                IntPtr hWnd = IntPtr.Zero;
                hWnd = objProcesses[0].MainWindowHandle;
                ShowWindowAsync(new HandleRef(null, hWnd), SW_RESTORE);
                SetForegroundWindow(objProcesses[0].MainWindowHandle);
            }
        }

        [DllImport("user32.dll")]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        static extern uint GetCurrentThreadId();

        [DllImport("user32.dll")]
        static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();


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
