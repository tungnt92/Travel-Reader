using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AppTourDuLich.DataModel
{
    class RssReader
    {
        public List<RssResult> KetQua(String xml)
        {
            try
            {
                XDocument xmlDocument = XDocument.Parse(xml);
                return (
                    from kq in xmlDocument.Descendants("item")
                    //where (kq.Element("link").Value.Substring(kq.Element("link").Value.LastIndexOf("/") + 1, kq.Element("link").Value.LastIndexOf(".") - (kq.Element("link").Value.LastIndexOf("/") + 1))).Equals("2-4-2015")
                    select new RssResult
                    {
                        Title = kq.Element("title").Value,
                        Date = kq.Element("pubDate").Value,
                        Description = kq.Element("description").Value,
                        Link = kq.Element("link").Value
                    }).ToList();
            }
            catch
            {
                // Return null to signify failure.
                return null;
            }
        }
    }
}
