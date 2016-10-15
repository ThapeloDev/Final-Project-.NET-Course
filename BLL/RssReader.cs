using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;
using BEL;

namespace BLL
{
    public class RssReader
    {
        private const string _rssUrl = @"http://www.ynet.co.il/Integration/StoryRss975.xml";
        public List<RssMessage> MsgList { get; set; }
        private RssMessage newRss;
        public RssReader()
        {
            MsgList = new List<RssMessage>();
            try
            {
                var reader = XmlReader.Create(_rssUrl);
                var syn = SyndicationFeed.Load(reader);

                foreach (var item in syn.Items)
                {
                    newRss = new RssMessage
                    {
                        _title = item.Title.Text,
                        _url = item.Links.First().Uri.AbsoluteUri
                    };
                    MsgList.Add(newRss);
                }
            }
            catch (Exception)
            {
                MsgList.Add(new RssMessage
                {
                    _title = "אין חיבור לאינטרנט",
                    _url = ""
                });
            }
        }
        public RssMessage GetRssMessage()
        {
            Random rnd = new Random();
            return MsgList.ElementAt(rnd.Next(0, MsgList.Count));
        }
    }
    public struct RssMessage
    {
        public string _title;
        public string _url;
        
        public void OpenLink()
        {
            try
            {
                System.Diagnostics.Process.Start(_url);
            }
            catch
            {
                // ignored
            }
        }
    }
}
