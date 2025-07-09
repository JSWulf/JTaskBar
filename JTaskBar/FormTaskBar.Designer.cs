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
            LiVw_Apps = new ListView();
            Window = new ColumnHeader();
            Menu_Main = new ContextMenuStrip(components);
            exitToolStripMenuItem = new ToolStripMenuItem();
            Timer_Clock = new System.Windows.Forms.Timer(components);
            TT_Win = new ToolTip(components);
            tableLayoutPanel1.SuspendLayout();
            Menu_Main.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(Btn_Menu, 0, 0);
            tableLayoutPanel1.Controls.Add(Btn_Desktop, 0, 5);
            tableLayoutPanel1.Controls.Add(Lab_Clock, 0, 3);
            tableLayoutPanel1.Controls.Add(LiVw_Apps, 0, 1);
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
            // 
            // LiVw_Apps
            // 
            LiVw_Apps.BackColor = Color.Black;
            LiVw_Apps.BorderStyle = BorderStyle.None;
            LiVw_Apps.Columns.AddRange(new ColumnHeader[] { Window });
            LiVw_Apps.Dock = DockStyle.Fill;
            LiVw_Apps.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            LiVw_Apps.ForeColor = Color.White;
            LiVw_Apps.FullRowSelect = true;
            LiVw_Apps.HeaderStyle = ColumnHeaderStyle.None;
            LiVw_Apps.Location = new Point(4, 44);
            LiVw_Apps.Margin = new Padding(4, 3, 4, 3);
            LiVw_Apps.MultiSelect = false;
            LiVw_Apps.Name = "LiVw_Apps";
            LiVw_Apps.Scrollable = false;
            LiVw_Apps.Size = new Size(91, 1063);
            LiVw_Apps.TabIndex = 3;
            LiVw_Apps.UseCompatibleStateImageBehavior = false;
            LiVw_Apps.View = View.Details;
            LiVw_Apps.MouseDown += LiVw_Apps_MouseDown;
            LiVw_Apps.MouseMove += LiVw_Apps_MouseMove;
            // 
            // Window
            // 
            Window.Width = 200;
            // 
            // Menu_Main
            // 
            Menu_Main.Items.AddRange(new ToolStripItem[] { exitToolStripMenuItem });
            Menu_Main.Name = "Menu_Main";
            Menu_Main.Size = new Size(94, 26);
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(93, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // Timer_Clock
            // 
            Timer_Clock.Enabled = true;
            Timer_Clock.Interval = 1000;
            Timer_Clock.Tick += Timer_Clock_Tick;
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
            TopMost = true;
            Load += FormTaskBar_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            Menu_Main.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button Btn_Menu;
        private System.Windows.Forms.Button Btn_Desktop;
        private System.Windows.Forms.Label Lab_Clock;
        private System.Windows.Forms.ListView LiVw_Apps;
        private System.Windows.Forms.ContextMenuStrip Menu_Main;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Timer Timer_Clock;
        private System.Windows.Forms.ToolTip TT_Win;
        private ColumnHeader Window;
    }
}

