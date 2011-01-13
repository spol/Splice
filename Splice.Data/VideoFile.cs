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

        public TVShow Show { get; set; }
        public int Season { get; set; }
        public int Episode { get; set; }
    }
}
