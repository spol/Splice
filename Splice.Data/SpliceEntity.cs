using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Splice.Data
{
    public enum SpliceEntityType
    {
        Collection,
        TVShow,
        TVSeason,
        TVEpisode,
        Movie
    }

    class SpliceEntity
    {
        public Int32 Id { get; set; }
    }
}
