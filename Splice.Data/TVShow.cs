using System;
using System.Collections.Generic;
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
        public float Rating { get; set; }
        public int Year { get; set; }
        public string Thumb { get; set; }
        public string Art { get; set; }
        public string Banner { get; set; }
        public int Duration { get; set; }
        public DateTime OriginallyAvailableAt { get; set; }
        public int Collection { get; set; }
        public int LeafCount { get; set; }
        public int ViewedLeafCount { get; set; }
        public int LastUpdated { get; set; }
        // Index?

        public string GetMedia(string mediaType)
        {
            switch (mediaType.ToLower())
            {
                case "thumb":
                    return Thumb;
                case "art":
                    return Art;
                case "banner":
                    return Banner;
                default:
                    throw new Exception("Invalid media type.");
            }
        }
    }
}
