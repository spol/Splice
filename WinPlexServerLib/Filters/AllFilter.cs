using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinPlexServer.Filters
{
    class AllFilter : Filter
    {
        public override string Name
        {
            get { return "All Shows"; }
        }

        public override string Key
        {
            get { return "all"; }
        }
    }
}
