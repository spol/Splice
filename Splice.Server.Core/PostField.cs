using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Splice.Server
{
    public class PostField
    {
        protected String _Name;
        public String Name
        {
            get
            {
                return _Name;
            }
        }

        protected String _Value;
        public String Value
        {
            get
            {
                return _Value;
            }
        }
    }
}
