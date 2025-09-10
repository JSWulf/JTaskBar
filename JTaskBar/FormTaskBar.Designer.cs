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
namespace JTaskBar
{
    partial class FormTaskBar
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            tableLayoutPanel1 = new TableLayoutPanel();
            Btn_Menu = new Button();
            Btn_Desktop = new Button();
            Lab_Clock = new Label();
            DGV_Apps = new DataGridView();
            Menu_Main = new ContextMenuStrip(components);
            Menu_Redraw = new ToolStripMenuItem();
            Menu_LRToggle = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripSeparator();
            Menu_Exit = new ToolStripMenuItem();
            Timer_Clock = new System.Windows.Forms.Timer(components);
            TT_Win = new ToolTip(components);
            Menu_WindowBtn = new ContextMenuStrip(components);
            MBtn_Restore = new ToolStripMenuItem();
            MBtn_Minimize = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            MBtn_Location = new ToolStripMenuItem();
            MBtn_TskMn = new ToolStripMenuItem();
            toolStripMenuItem4 = new ToolStripSeparator();
            MBtn_Close = new ToolStripMenuItem();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)DGV_Apps).BeginInit();
            Menu_Main.SuspendLayout();
            Menu_WindowBtn.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(Btn_Menu, 0, 0);
            tableLayoutPanel1.Controls.Add(Btn_Desktop, 0, 5);
            tableLayoutPanel1.Controls.Add(Lab_Clock, 0, 3);
            tableLayoutPanel1.Controls.Add(DGV_Apps, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(4, 3, 4, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 6;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(99, 1156);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // Btn_Menu
            // 
            Btn_Menu.Anchor = AnchorStyles.Top;
            Btn_Menu.Location = new Point(21, 3);
            Btn_Menu.Margin = new Padding(4, 3, 4, 3);
            Btn_Menu.Name = "Btn_Menu";
            Btn_Menu.Size = new Size(57, 35);
            Btn_Menu.TabIndex = 0;
            Btn_Menu.Text = "Menu";
            Btn_Menu.UseVisualStyleBackColor = true;
            Btn_Menu.Click += Btn_Menu_Click;
            // 
            // Btn_Desktop
            // 
            Btn_Desktop.Dock = DockStyle.Bottom;
            Btn_Desktop.Location = new Point(4, 1126);
            Btn_Desktop.Margin = new Padding(4, 3, 4, 3);
            Btn_Desktop.Name = "Btn_Desktop";
            Btn_Desktop.Size = new Size(91, 27);
            Btn_Desktop.TabIndex = 1;
            Btn_Desktop.Text = "Desktop";
            Btn_Desktop.UseVisualStyleBackColor = true;
            Btn_Desktop.Click += Btn_Desktop_Click;
            // 
            // Lab_Clock
            // 
            Lab_Clock.AutoSize = true;
            Lab_Clock.BackColor = Color.Transparent;
            Lab_Clock.Dock = DockStyle.Bottom;
            Lab_Clock.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Lab_Clock.ForeColor = Color.White;
            Lab_Clock.Location = new Point(4, 1110);
            Lab_Clock.Margin = new Padding(4, 0, 4, 0);
            Lab_Clock.Name = "Lab_Clock";
            Lab_Clock.Size = new Size(91, 13);
            Lab_Clock.TabIndex = 2;
            Lab_Clock.Text = "Clock Text";
            Lab_Clock.TextAlign = ContentAlignment.BottomCenter;
            Lab_Clock.Click += Lab_Clock_Click;
            // 
            // DGV_Apps
            // 
            DGV_Apps.AllowUserToAddRows = false;
            DGV_Apps.AllowUserToDeleteRows = false;
            DGV_Apps.AllowUserToResizeColumns = false;
            DGV_Apps.AllowUserToResizeRows = false;
            DGV_Apps.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            DGV_Apps.BackgroundColor = Color.Black;
            DGV_Apps.ColumnHeadersVisible = false;
            DGV_Apps.Dock = DockStyle.Fill;
            DGV_Apps.Location = new Point(3, 44);
            DGV_Apps.MultiSelect = false;
            DGV_Apps.Name = "DGV_Apps";
            DGV_Apps.ReadOnly = true;
            DGV_Apps.RowHeadersVisible = false;
            DGV_Apps.ScrollBars = ScrollBars.Vertical;
            DGV_Apps.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DGV_Apps.Size = new Size(93, 1063);
            DGV_Apps.TabIndex = 4;
            // 
            // Menu_Main
            // 
            Menu_Main.Items.AddRange(new ToolStripItem[] { Menu_Redraw, Menu_LRToggle, toolStripMenuItem2, Menu_Exit });
            Menu_Main.Name = "Menu_Main";
            Menu_Main.Size = new Size(166, 76);
            // 
            // Menu_Redraw
            // 
            Menu_Redraw.Name = "Menu_Redraw";
            Menu_Redraw.Size = new Size(165, 22);
            Menu_Redraw.Text = "Re-Draw";
            Menu_Redraw.Click += reDrawToolStripMenuItem_Click;
            // 
            // Menu_LRToggle
            // 
            Menu_LRToggle.Name = "Menu_LRToggle";
            Menu_LRToggle.Size = new Size(165, 22);
            Menu_LRToggle.Text = "Toggle Left/Right";
            Menu_LRToggle.Click += Menu_LRToggle_Click;
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(162, 6);
            // 
            // Menu_Exit
            // 
            Menu_Exit.Name = "Menu_Exit";
            Menu_Exit.Size = new Size(165, 22);
            Menu_Exit.Text = "Exit";
            Menu_Exit.Click += exitToolStripMenuItem_Click;
            // 
            // Timer_Clock
            // 
            Timer_Clock.Enabled = true;
            Timer_Clock.Interval = 1000;
            Timer_Clock.Tick += Timer_Clock_Tick;
            // 
            // Menu_WindowBtn
            // 
            Menu_WindowBtn.Items.AddRange(new ToolStripItem[] { MBtn_Restore, MBtn_Minimize, toolStripMenuItem1, MBtn_Location, MBtn_TskMn, toolStripMenuItem4, MBtn_Close });
            Menu_WindowBtn.Name = "Menu_WindowBtn";
            Menu_WindowBtn.Size = new Size(179, 126);
            // 
            // MBtn_Restore
            // 
            MBtn_Restore.Name = "MBtn_Restore";
            MBtn_Restore.Size = new Size(178, 22);
            MBtn_Restore.Text = "Restore";
            MBtn_Restore.Click += MBtn_Restore_Click;
            // 
            // MBtn_Minimize
            // 
            MBtn_Minimize.Name = "MBtn_Minimize";
            MBtn_Minimize.Size = new Size(178, 22);
            MBtn_Minimize.Text = "Minimize";
            MBtn_Minimize.Click += MBtn_Minimize_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(175, 6);
            // 
            // MBtn_Location
            // 
            MBtn_Location.Name = "MBtn_Location";
            MBtn_Location.Size = new Size(178, 22);
            MBtn_Location.Text = "Open File Location";
            MBtn_Location.Click += MBtn_Location_Click;
            // 
            // MBtn_TskMn
            // 
            MBtn_TskMn.Name = "MBtn_TskMn";
            MBtn_TskMn.Size = new Size(178, 22);
            MBtn_TskMn.Text = "Open Task Manager";
            MBtn_TskMn.Click += MBtn_TskMn_Click;
            // 
            // toolStripMenuItem4
            // 
            toolStripMenuItem4.Name = "toolStripMenuItem4";
            toolStripMenuItem4.Size = new Size(175, 6);
            // 
            // MBtn_Close
            // 
            MBtn_Close.Name = "MBtn_Close";
            MBtn_Close.Size = new Size(178, 22);
            MBtn_Close.Text = "Close";
            MBtn_Close.Click += MBtn_Close_Click;
            // 
            // FormTaskBar
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(99, 1156);
            ControlBox = false;
            Controls.Add(tableLayoutPanel1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Margin = new Padding(4, 3, 4, 3);
            Name = "FormTaskBar";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            Load += FormTaskBar_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)DGV_Apps).EndInit();
            Menu_Main.ResumeLayout(false);
            Menu_WindowBtn.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button Btn_Menu;
        private System.Windows.Forms.Button Btn_Desktop;
        private System.Windows.Forms.Label Lab_Clock;
        private System.Windows.Forms.ContextMenuStrip Menu_Main;
        private System.Windows.Forms.ToolStripMenuItem Menu_Exit;
        private System.Windows.Forms.Timer Timer_Clock;
        private System.Windows.Forms.ToolTip TT_Win;
        private ContextMenuStrip Menu_WindowBtn;
        private ToolStripMenuItem MBtn_Restore;
        private ToolStripMenuItem MBtn_Minimize;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem MBtn_Location;
        private ToolStripMenuItem MBtn_TskMn;
        private ToolStripSeparator toolStripMenuItem4;
        private ToolStripMenuItem MBtn_Close;
        private ToolStripMenuItem Menu_Redraw;
        private DataGridView DGV_Apps;
        private ToolStripMenuItem Menu_LRToggle;
        private ToolStripSeparator toolStripMenuItem2;
    }
}

