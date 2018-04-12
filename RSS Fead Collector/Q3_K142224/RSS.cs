using System;
using System.Xml;
using System.ServiceModel.Syndication;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml.Linq;

namespace Q3_K142224
{
    [SerializableAttribute]
    class RSS
    {
        public RSS()
        {
            ServiceLog.WriteLog("in feed ");
            string url1 = "https://www.samaa.tv/feed/";
            XmlReader reader1 = XmlReader.Create(url1);
            SyndicationFeed feed1 = SyndicationFeed.Load(reader1);
            string source = feed1.Title.Text;

            reader1.Close();

            // 2ND FEED

            string url2 = "https://www.geo.tv/rss/1/0";
            XmlReader reader2 = XmlReader.Create(url2);
            SyndicationFeed feed2 = SyndicationFeed.Load(reader2);
            string source2 = feed2.Title.Text;

            reader2.Close();



            foreach (SyndicationItem item in feed1.Items)
            {
                string subject = item.Title.Text;
                string description = item.Summary.Text;

                string time = item.PublishDate.ToString();



                ServiceLog.WriteLog("in feed  before xml call");
                string disc = clean_tags(description, item);
                write_to_xml(feed1, item);

                Console.WriteLine("{0} \n ::: {1} \n:: {2} \n {3}\n", subject, disc, source, time);

                
            }



            foreach (SyndicationItem item in feed2.Items)
            {
                string subject = item.Title.Text;
                string description = item.Summary.Text;

                string time = item.PublishDate.ToString();




                string disc = clean_tags(description, item);
                write_to_xml(feed2, item);

                Console.WriteLine("{0} \n ::: {1} \n:: {2} \n {3}\n", subject, disc, source, time);

                /* try
                 {
                     XmlWriter xmlWriter = XmlWriter.Create("TestRSSFile.xml");
                     feed.SaveAsRss20(xmlWriter);
                 }

                 catch { }*/
            }


        }

        public static void write_to_xml(SyndicationFeed feed, SyndicationItem item)
        {
            try
            {
                ServiceLog.WriteLog("in xml writer");
                string file = @"C:\Users\TAHA TARIQ\Documents\Visual Studio 2015\Projects\Q3_K142224\Q3_K142224\bin\Debug\news.xml";
                bool newFile = false;

            
                if (!File.Exists(file))
                {
                    newFile = true;

                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Indent = true;
                    settings.IndentChars = ("    ");
                    settings.CloseOutput = true;
                    settings.OmitXmlDeclaration = true;
                    settings.NewLineChars = "\n";


                   
                    using (XmlWriter writer = XmlWriter.Create(file, settings))
                    {


                        writer.WriteStartElement("NewsItem");

                        writer.WriteElementString("Title", item.Title.Text);
                        writer.WriteElementString("Description", item.Summary.Text);
                        writer.WriteElementString("PublishedDate", item.PublishDate.ToString());
                        writer.WriteElementString("NewsChannel", feed.Title.Text);

                       
                        writer.WriteEndElement();
                        writer.Flush();
                    }
                }


                else
                {
                    
                    XDocument doc = XDocument.Load(file);
                    XElement NewsItem = doc.Element("NewsItem");
                    NewsItem.Add(
                        new XElement("NewsItem",
                               new XElement("Title", item.Title.Text),
                               new XElement("Description", item.Summary.Text),
                               new XElement("PublishedDate", item.PublishDate.ToString())),
                               new XElement("NewsChannel", feed.Title.Text)
                              );

                    
                    doc.Save(file);

                }
            }

            catch (Exception e)
            {
                ServiceLog.WriteLog(e.ToString());
                

            }


        }


        static string clean_tags(string description, SyndicationItem item)
        {
            try
            { // Get rid of the tags
                
                description = Regex.Replace(description, @"<.+?>", String.Empty);
                // Then decode the HTML entities
                description = WebUtility.HtmlDecode(description);

                return description;
            }
            catch
            {

                return ("invalid argument");
            }

        }
    }
}
