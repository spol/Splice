using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;

namespace Splice.Data
{
    public class TVShow
    {
        public string Title { get; set; }
        public int Id { get; set; }
        public string Studio { get; set; }
        public string Type { get { return "show"; } }
        public string ContentRating { get; set; }
        public string Summary { get; set; }
        public double Rating { get; set; }
        public int Year { get { return AirDate.Year; } }
        public string Thumb { get; set; }
        public string Art { get; set; }
        public string Banner { get; set; }
        public int Duration { get; set; }
        public DateTime AirDate { get; set; }
        public int Collection { get; set; }
        public int LeafCount { get; set; }
        public int ViewedLeafCount { get; set; }
        public int LastUpdated { get; set; }
        public string Location { get; set; }
        public int TvdbId { get; set; }
        public bool DvdOrder { get; set; }
        // Index?

        public TVShow() { }

        public TVShow(DataRow Row)
        {
            Id = Convert.ToInt32(Row["id"]);
            Collection = Convert.ToInt32(Row["collection"]);
            Title = Convert.ToString(Row["title"]);
            Banner = Convert.ToString(Row["banner"] == DBNull.Value ? "" : Row["banner"]);
            Art = Convert.ToString(Row["art"] == DBNull.Value ? "" : Row["art"]);
            Thumb = Convert.ToString(Row["thumb"] == DBNull.Value ? "" : Row["thumb"]);
            LastUpdated = Convert.ToInt32(Row["lastUpdated"]);
            Summary = Row["summary"].ToString();
            TvdbId = Convert.ToInt32(Row["tvdbId"]);
            DvdOrder = Convert.ToInt32(Row["dvdOrder"]) != 0;
            Location = Row["location"].ToString();
        }
    }
}
