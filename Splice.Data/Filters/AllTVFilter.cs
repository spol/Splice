using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Splice.Data.Filters
{
    class AllTVFilter : Filter
    {
        public override string Name
        {
            get { return "All Shows"; }
        }

        public override string Key
        {
            get { return "all"; }
        }

        public override string Query
        {
            get { return @"SELECT * FROM tv_shows WHERE collection = {0}"; }
        }
    }
}
