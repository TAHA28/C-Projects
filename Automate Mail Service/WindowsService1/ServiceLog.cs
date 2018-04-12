using System;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Xml;

namespace WindowsService1
{
    class ServiceLog
    {  
        /// This function write log to LogFile.text  
         
        public static void WriteLog(string Message)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + Message);
                sw.Flush();
                sw.Close();
            }
            catch
            {
            }
        }

        static int i = 1;
        public static void ReadXml()
        {
            WriteLog("xml wala");
            string path = ConfigurationManager.AppSettings["Path"].ToString();
            string path2 = i + ".xml";
            string dest = path + path2;


            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(dest);

            string[] text = new string[3];
            int j = 0;
            foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
            {
                text[j++] = node.InnerText;


            }

            string to = text[0];
            string subject = text[1];
            string msg = text[2];
            i++ ;
            SendEmail(to,subject,msg);



        }
        /// Send Email given subject and message.  
        
        public static void SendEmail(String ToEmail, String Subj, string Message)
        {
                 
            string FromEmailid = ConfigurationManager.AppSettings["FromMail"].ToString();
            string Pass = ConfigurationManager.AppSettings["Password"].ToString();


            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;

            string body = Message;
            client.Credentials = new System.Net.NetworkCredential(FromEmailid, Pass);

            MailMessage mm = new MailMessage(FromEmailid, "k142224@nu.edu.pk", Subj, body);
            String smsg = "Email sent successfully !!!";
            WriteLog(smsg);
            mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            client.Send(mm);

        }

        
    }
}
