using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Splice.Server
{
    public class PostData
    {
        public Dictionary<String, PostField> Fields = new Dictionary<String, PostField>();

        public PostField this[String Name]
        {
            get
            {
                if (Fields.Keys.Contains(Name))
                {
                    return Fields[Name];
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
