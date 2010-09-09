using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinPlexServer.Filters;

namespace WinPlexServer
{
    public abstract class Filter
    {
        private static List<Filter> tvFilters;

        public abstract string Name { get; }

        public abstract string Key { get; }

        public abstract string Query { get; }

        public static List<Filter> GetTVFilterList()
        {
            tvFilters.Add(new AllTVFilter());

            return tvFilters;
        }

        public static Filter GetTVFilter(string key)
        {
            foreach (Filter filter in tvFilters)
            {
                if (filter.Key == key)
                {
                    return filter;
                }
            }
            return null;
        }
    }
}
