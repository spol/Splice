using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Splice.Data
{
    public class TVEpisode
    {
        public Int32 Id { get; set; }
        public Int32 TvdbId { get; set; }
        public string Type { get { return "episode"; } }
        public Int32 SeasonId { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public Int32 EpisodeNumber { get; set; }
        public double Rating { get; set; }
        public string Thumb { get; set; }
        public Int32 Duration
        {
            get
            {
                Int32 TotalDuration = 0;
                foreach (VideoFileInfo File in VideoFiles)
                {
                    TotalDuration += File.Duration;
                }
                return TotalDuration;
            }
        }
        public DateTime AirDate { get; set; }
        public Int32 LastUpdated { get { return 1; } }
        public List<VideoFileInfo> VideoFiles { get; set; }

        public TVEpisode()
        {
            VideoFiles = new List<VideoFileInfo>();
        }

        public TVEpisode(DataRow Row)
        {
            AirDate = Convert.ToDateTime(Row["airDate"]);
            EpisodeNumber = Convert.ToInt32(Row["episodeNumber"]);
            Id = Convert.ToInt32(Row["id"]);
            Rating = Convert.ToSingle(Row["rating"]);
            SeasonId = Convert.ToInt32(Row["seasonId"]);
            Summary = Row["Summary"].ToString();
            Title = Row["title"].ToString();
            Thumb = Row["thumb"].ToString();
            VideoFiles = new List<VideoFileInfo>();
        }
    }
}
