using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinPlexServer
{
    class TVEpisode
    {
        public Int32 Id { get; set; }
        public string Type { get { return "episode"; } }
        public Int32 SeasonId { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public Int32 EpisodeNumber { get; set; }
        public float Rating { get; set; }
        public Int32 Duration { get; set; }
        public DateTime AirDate { get; set; }
        public Int32 LastUpdated { get { return 1; } }
        public VideoFile VideoFile { get; set; }
    }
}
