using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Splice.Data
{
    public class VideoFileInfo : MediaFileInfo
    {
        public Int32 Bitrate { get; set; }
        public Double AspectRatio { get; set; }
        public Double AudioChannels { get; set; }
        public string AudioCodec { get; set; }
        public string VideoCodec { get; set; }
        public string VideoResolution { get; set; }
        public Double VideoFrameRate { get; set; }

        public VideoFileInfo(string Filename) : base(Filename) { }

        public VideoFileInfo(DataRow Row) : base(Row)
        {
            Bitrate = Convert.ToInt32(Row["bitrate"]);
            AspectRatio = Convert.ToDouble(Row["aspectRatio"]);
            AudioChannels = Convert.ToDouble(Row["audioChannels"]);
            AudioCodec = Row["audioCodec"].ToString();
            VideoCodec = Row["videoCodec"].ToString();
            VideoFrameRate = Row["videoFrameRate"] == DBNull.Value ? 0 : Convert.ToDouble(Row["videoFrameRate"]);
            VideoResolution = Row["videoResolution"].ToString();
        }
    }
}
