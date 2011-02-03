using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Splice.Manager
{
    public class Collection
    {
        public String Name { get; set; }
        public List<String> Paths { get; set; }
        public String Type { get; set; }
        public Bitmap Artwork { get; set; }

        public NameValueCollection ToNameValueCollection()
        {
            NameValueCollection Fields = new NameValueCollection();
            Fields.Add("Name", Name);
            Fields.Add("Type", Type);

            foreach (String Path in Paths)
            {
                Fields.Add("Paths", Path);
            }

            return Fields;
        }
    }
}
