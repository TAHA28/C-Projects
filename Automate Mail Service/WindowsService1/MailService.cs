using System;
using System.Configuration;
using System.ServiceProcess;
using System.Timers;

namespace WindowsService1
{
    public partial class MailService : ServiceBase
    {
        Timer timer;
        private string timeString;

        public MailService()
        {
            InitializeComponent();
            timer = new Timer();

            int interval = 10000;
            timer.Interval = interval;
            timer.Elapsed += new ElapsedEventHandler(ServiceTimer_Tick);
        }
    

        private double GetNextInterval()
        {
            
            timeString = ConfigurationManager.AppSettings["StartTime"];
            DateTime t = DateTime.Parse(timeString);
            TimeSpan ts = new TimeSpan();
            ts = t -DateTime.Now;
            if (ts.TotalMilliseconds < 0)
            {
                ts = t.AddMinutes(1) - DateTime.Now; //Here you can increase the timer interval based on your requirments.   
            }
            return ts.TotalMilliseconds;
        }
       
        protected override void OnStart(string[] args)
        {
            timer.AutoReset = true;
            timer.Enabled = true;
            ServiceLog.WriteLog("Daily Reporting service started");
        }


        protected override void OnStop()
        {
            timer.AutoReset = false;

            timer.Enabled = false;
            ServiceLog.WriteLog("Daily Reporting service stopped");
        }
   

        private void ServiceTimer_Tick(object sender,ElapsedEventArgs e)
        {


           
            //string Msg = "Hi ! This is Daily Mail Service";

            //ServiceLog.SendEmail("taha2827@gmail.com", "Daily Report of DailyMailSchedulerService on " + DateTime.Now.ToString("dd-MMM-yyyy"), Msg);
            ServiceLog.ReadXml();
                timer.Stop();
                System.Threading.Thread.Sleep(6000);
                SetTimer();
            }

        private void SetTimer()
        {

                
                double inter = GetNextInterval();
                timer.Interval = inter;
                ServiceLog.WriteLog("got interval::" +  inter);
                timer.Start();    
                timer.Elapsed += new ElapsedEventHandler(ServiceTimer_Tick);
                ServiceLog.WriteLog("got elapsed");
        }
    }

         


    }

