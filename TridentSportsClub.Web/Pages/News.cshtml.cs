using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace TridentSportsClub.Web.Pages
{
    public class NewsModel : PageModel
    {
        public string Message { get; set; }
        public List<SCFNews> SCFNewsCollection { get; set; }
        public class SCFNews
        {
            public string Title { get; set; }

            
            public string ReleaseDate { get; set; }
            public string Description { get; set; }

           
            public string URL { get; set; }
        }

        public void OnGet()
        {
            Message = "Your application description page.";
            SCFNewsCollection = GetRSS();
        }
        public static List<SCFNews> GetRSS()
        {
            try
            {
                XmlDocument newsUrl = new XmlDocument();
                newsUrl.Load("http://www.swedishcricket.org/Nyheter/SCFNyheter?rss=True&complete=2");
                XDocument doc = XDocument.Parse(newsUrl.InnerXml);
                var docs = doc.Root.Element("channel").ToString();
                newsUrl.LoadXml(docs);
                XmlNodeList idNodes = newsUrl.SelectNodes("channel/item");
                StringBuilder sb = new StringBuilder();
                int count = 0;
                count = idNodes.Count;
                List<SCFNews> collNews = new List<SCFNews>();
                foreach (XmlNode node in idNodes)
                {
                    SCFNews oNews = new SCFNews();
                    oNews.Title = node["title"].InnerText;
                    oNews.URL = node["link"].InnerText;
                    oNews.Description = node["mainIntro"].InnerText;
                    oNews.ReleaseDate = node["pubDate"].InnerText;
                    collNews.Add(oNews);
                }
                return collNews;
            }
            catch (Exception ex)
            {
                NewsModel m = new NewsModel();
                m.Message = ex.Message;
                return null;
            }
        }
    }
}
