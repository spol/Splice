using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinPlexServer.Filters;

namespace WinPlexServer
{
    public abstract class Filter
    {
        public abstract string Name
        {
            get;
        }

        public abstract string Key
        {
            get;
        }

        public static List<Filter> GetList()
        {
            List<Filter> filters = new List<Filter>();
            filters.Add(new AllFilter());

            return filters;
        }
    }
}
