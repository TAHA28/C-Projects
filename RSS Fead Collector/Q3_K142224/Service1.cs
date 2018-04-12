using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Q3_K142224
{
    public partial class Service1 : ServiceBase
    {
        Timer timer;
        public Service1()
        {
            InitializeComponent();
            timer = new Timer();
            int interval = 10000;
            timer.Interval = interval;
            timer.Elapsed += new ElapsedEventHandler(ServiceTimer_Tick);
        }

        private void ServiceTimer_Tick(object sender, ElapsedEventArgs e)
        {
            RSS obj = new RSS();
            timer.Stop();
            System.Threading.Thread.Sleep(6000);
            SetTimer();
        }

        protected override void OnStart(string[] args)
        {
            timer.AutoReset = true;
            timer.Enabled = true;
            ServiceLog.WriteLog("Rss Fead Creator");
        }

        protected override void OnStop()
        {
            timer.AutoReset = false;

            timer.Enabled = false;
            ServiceLog.WriteLog("Service stopped");
        }
        private void SetTimer()
        {
 
            timer.Interval = 300000;         
            timer.Start();
            timer.Elapsed += new ElapsedEventHandler(ServiceTimer_Tick);
            ServiceLog.WriteLog("got elapsed");
        }
    }
}
