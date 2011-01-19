using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Splice.Data
{
    public enum VideoCollectionType
    {
        show,
        movie
    }

    public class VideoCollection
    {
        public string Art { get; set; }
        public string Title { get; set; }
        public int Id { get; set; }
        public List<string> Locations { get; set; }
        public VideoCollectionType Type { get; set; }

        public VideoCollection()
        {
            Locations = new List<string>();
        }

        public VideoCollection(DataRow Row)
        {
            Locations = new List<string>();
            Id = Convert.ToInt32(Row["id"]);
            Type = (VideoCollectionType)Enum.Parse(typeof(VideoCollectionType), Row["type"].ToString());
            Title = Row["title"].ToString();
            Art = Row["art"].ToString();
        }
    }
}
