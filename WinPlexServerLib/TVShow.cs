using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace WinPlexServer
{
    public class TVShow
    {
        public string Title { get; set; }
        public int Id { get; set; }
        public string Studio { get; set; }
        public int RatingKey { get { return Id; } }
        public string Type { get { return "show"; } }
        public string Key { get { return "/library/metadata/" + Id.ToString() + "/children"; } }
        public string ContentRating { get; set; }
        public string Summary { get; set; }
        public float Rating { get; set; }
        public int Year { get; set; }
        public string Thumb { get; set; }
        public string Art { get; set; }
        public string Banner { get; set; }
        public int Duration { get; set; }
        public DateTime OriginallyAvailableAt { get; set; }
        public int Collection { get; set; }

        // Index
        // LeafCount
        // ViewedLeafCount

        public XmlElement ToXml(XmlDocument doc)
        {
            XmlElement el = doc.CreateElement("Directory");
            el.SetAttribute("ratingKey", RatingKey.ToString());
            el.SetAttribute("key", Key);
            el.SetAttribute("studio", Studio);
            el.SetAttribute("type", Type);
            el.SetAttribute("title", Title);
            el.SetAttribute("contentRating", ContentRating);
            el.SetAttribute("summary", Summary);
            el.SetAttribute("rating", Rating.ToString());
            el.SetAttribute("year", Year.ToString());
            el.SetAttribute("thumb", Thumb);
            el.SetAttribute("art", Art);
            el.SetAttribute("banner", Banner);
            el.SetAttribute("duration", Duration.ToString());
            el.SetAttribute("originallyAvailableAt", OriginallyAvailableAt.ToShortDateString());
            el.SetAttribute("leafCount", "2");
            el.SetAttribute("viewedLeafCount", "0");
            return el;
        }
    }
}
