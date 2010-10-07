using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Splice.Watcher
{
    static class Extensions
    {
        public static int Timestamp(this DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date - origin;
            return Convert.ToInt32(Math.Floor(diff.TotalSeconds));
        }
    }
}
