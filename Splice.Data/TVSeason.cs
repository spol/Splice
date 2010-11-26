using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Splice.Data
{
    public class TVSeason
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int SeasonNumber { get; set; }
        public Int32 ShowId { get; set; }
        public string Art { get; set; }
    }
}
