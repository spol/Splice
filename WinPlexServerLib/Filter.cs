using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinPlexServer.Filters;

namespace WinPlexServer
{
    public abstract class Filter
    {
        private static List<Filter> tvFilters {
            get {
                List<Filter> filters = new List<Filter>();
                filters.Add(new AllTVFilter());
                return filters;
            }
        }

        public abstract string Name { get; }

        public abstract string Key { get; }

        public abstract string Query { get; }

        public static List<Filter> GetTVFilterList()
        {
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
