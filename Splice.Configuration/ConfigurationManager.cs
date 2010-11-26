using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Splice.Configuration
{
    public static class ConfigurationManager
    {
        public static List<string> VideoExtensions
        {
            get
            {
                List<string> Extensions = new List<string>();
                Extensions.Add(".txt");

                return Extensions;
            }
        }
    }
}
