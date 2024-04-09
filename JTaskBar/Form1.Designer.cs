namespace JTaskBar
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.Btn_Menu = new System.Windows.Forms.Button();
            this.Btn_Desktop = new System.Windows.Forms.Button();
            this.Lab_Clock = new System.Windows.Forms.Label();
            this.LiBx_Apps = new System.Windows.Forms.ListBox();
            this.Menu_Main = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Timer_Clock = new System.Windows.Forms.Timer(this.components);
            this.TT_Win = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.Menu_Main.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.Btn_Menu, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.Btn_Desktop, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.Lab_Clock, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.LiBx_Apps, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(79, 1002);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // Btn_Menu
            // 
            this.Btn_Menu.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Btn_Menu.Location = new System.Drawing.Point(15, 3);
            this.Btn_Menu.Name = "Btn_Menu";
            this.Btn_Menu.Size = new System.Drawing.Size(49, 30);
            this.Btn_Menu.TabIndex = 0;
            this.Btn_Menu.Text = "Menu";
            this.Btn_Menu.UseVisualStyleBackColor = true;
            this.Btn_Menu.Click += new System.EventHandler(this.Btn_Menu_Click);
            // 
            // Btn_Desktop
            // 
            this.Btn_Desktop.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Btn_Desktop.Location = new System.Drawing.Point(3, 976);
            this.Btn_Desktop.Name = "Btn_Desktop";
            this.Btn_Desktop.Size = new System.Drawing.Size(73, 23);
            this.Btn_Desktop.TabIndex = 1;
            this.Btn_Desktop.Text = "Desktop";
            this.Btn_Desktop.UseVisualStyleBackColor = true;
            this.Btn_Desktop.Click += new System.EventHandler(this.Btn_Desktop_Click);
            // 
            // Lab_Clock
            // 
            this.Lab_Clock.AutoSize = true;
            this.Lab_Clock.BackColor = System.Drawing.Color.Transparent;
            this.Lab_Clock.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Lab_Clock.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lab_Clock.ForeColor = System.Drawing.Color.White;
            this.Lab_Clock.Location = new System.Drawing.Point(3, 960);
            this.Lab_Clock.Name = "Lab_Clock";
            this.Lab_Clock.Size = new System.Drawing.Size(73, 13);
            this.Lab_Clock.TabIndex = 2;
            this.Lab_Clock.Text = "Clock Text";
            this.Lab_Clock.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // LiBx_Apps
            // 
            this.LiBx_Apps.BackColor = System.Drawing.Color.Black;
            this.LiBx_Apps.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.LiBx_Apps.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LiBx_Apps.ForeColor = System.Drawing.Color.White;
            this.LiBx_Apps.FormattingEnabled = true;
            this.LiBx_Apps.ItemHeight = 15;
            this.LiBx_Apps.Items.AddRange(new object[] {
            "Window 1",
            "Window 2",
            "etc."});
            this.LiBx_Apps.Location = new System.Drawing.Point(3, 39);
            this.LiBx_Apps.Name = "LiBx_Apps";
            this.LiBx_Apps.Size = new System.Drawing.Size(73, 855);
            this.LiBx_Apps.TabIndex = 3;
            this.LiBx_Apps.SelectedIndexChanged += new System.EventHandler(this.LiBx_Apps_SelectedIndexChanged);
            this.LiBx_Apps.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LiBx_Apps_MouseMove);
            // 
            // Menu_Main
            // 
            this.Menu_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.Menu_Main.Name = "Menu_Main";
            this.Menu_Main.Size = new System.Drawing.Size(94, 26);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(93, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // Timer_Clock
            // 
            this.Timer_Clock.Enabled = true;
            this.Timer_Clock.Interval = 1000;
            this.Timer_Clock.Tick += new System.EventHandler(this.Timer_Clock_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(79, 1002);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.Menu_Main.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button Btn_Menu;
        private System.Windows.Forms.Button Btn_Desktop;
        private System.Windows.Forms.Label Lab_Clock;
        private System.Windows.Forms.ListBox LiBx_Apps;
        private System.Windows.Forms.ContextMenuStrip Menu_Main;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Timer Timer_Clock;
        private System.Windows.Forms.ToolTip TT_Win;
    }
}

