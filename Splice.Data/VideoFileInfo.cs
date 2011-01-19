using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Splice.MediaInfoLib;

namespace Splice.Data
{
    public class VideoFileInfo : MediaFileInfo
    {
        public Int32 Bitrate { get; set; }
        public Double AspectRatio { get; set; }
        public Double AudioChannels { get; set; }
        public String AudioCodec { get; set; }
        public String VideoCodec { get; set; }
        public Int32 PictureHeight { get; set; }
        public Int32 PictureWidth { get; set; }
        public Double FrameRate { get; set; }

        public VideoFileInfo(string Filename) : base(Filename) { }

        public VideoFileInfo(DataRow Row) : base(Row)
        {
            Bitrate = Convert.ToInt32(Row["bitrate"]);
            AspectRatio = Convert.ToDouble(Row["aspectRatio"]);
            AudioChannels = Convert.ToDouble(Row["audioChannels"]);
            AudioCodec = Row["audioCodec"].ToString();
            VideoCodec = Row["videoCodec"].ToString();
            FrameRate = Row["videoFrameRate"] == DBNull.Value ? 0 : Convert.ToDouble(Row["videoFrameRate"]);
            PictureWidth = Convert.ToInt32(Row["PictureWidth"]);
            PictureHeight = Convert.ToInt32(Row["PictureHeight"]);
        }

        public void LoadMetaDataFromFile()
        {
            MediaInfo Info = new MediaInfo();
            Info.Open(Path);
            //String Params = Info.Option("Info_Parameters");
            Bitrate = Convert.ToInt32(Info.Get(StreamKind.General, 0, "BitRate"));
            Duration = Convert.ToInt32(Info.Get(StreamKind.General, 0, "Duration"));

            VideoCodec = Info.Get(StreamKind.Video, 0, "Format");
            PictureWidth = Convert.ToInt32(Info.Get(StreamKind.Video, 0, "Width"));
            PictureHeight = Convert.ToInt32(Info.Get(StreamKind.Video, 0, "Height"));
            FrameRate = Convert.ToDouble(Info.Get(StreamKind.Video, 0, "FrameRate"));
            AspectRatio = Convert.ToDouble(Info.Get(StreamKind.Video, 0, "DisplayAspectRatio"));

            AudioCodec = Info.Get(StreamKind.Audio, 0, "Format");
            AudioChannels = Convert.ToDouble(Info.Get(StreamKind.Audio, 0, "Channel(s)"));

            Info.Close();
        }
    }
}
