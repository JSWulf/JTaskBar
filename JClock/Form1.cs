using System;
using System.Windows.Forms;


namespace JClock
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


        private void Timer_Clock_Tick(object sender, EventArgs e)
        {
            Lab_Clock.Text = DateTime.Now.ToString("HH:mm \ndddd\nyyyy-MM-dd") + "\n" + DateTime.Now.ISOWorkWeek();

        }



    }
}
