using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Splice.Data
{
    public class VideoFile
    {
        public Int32 Id { get; set; }
        public Int32 Duration { get; set; }
        public Int32 Bitrate { get; set; }
        public float AspectRatio { get; set; }
        public float AudioChannels { get; set; }
        public string AudioCodec { get; set; }
        public string VideoCodec { get; set; }
        public string VideoResolution { get; set; }
        public string VideoFrameRate { get; set; }
        public Int32 Size { get; set; }
        public string Path { get; set; }
    }
}
