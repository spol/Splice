using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace Splice.Server
{
    class MimePostField : PostField
    {
        private NameValueCollection _Headers = new NameValueCollection();
        public NameValueCollection Headers
        {
            get
            {
                return _Headers;
            }
        }

        public Boolean IsFile
        {
            get
            {
                return false;
            }
        }

        public MimePostField(String Data)
        {
            if (Data.StartsWith("\r\n\r\n"))
            {
                // no headers
                Data = Data.Substring(4);
            }
            else
            {
                // headers
                int Boundary = Data.IndexOf("\r\n\r\n");
                String PostFieldHeaders = Data.Substring(0, Boundary);
                Data = Data.Substring(Boundary + 4);

                List<String> Headers = PostFieldHeaders.Split(new String[] { "\r\n" }, StringSplitOptions.None).ToList<String>();

                foreach (String Header in Headers)
                {
                    String[] Fields = new String[2];
                    Fields = Header.Split(new Char[] {':'}, 2);

                    _Headers.Add(Fields[0], Fields[1]);

                    // check for name
                    if (Fields[0] == "Content-Disposition")
                    {
                        Regex R = new Regex("name=\"(.*)\"");

                        Match M = R.Match(Fields[1]);

                        if (M.Groups.Count > 1)
                        {
                            _Name = M.Groups[1].Value;
                        }
                    }

                }
            }

            _Value = Data.Substring(0, Data.Length-2);
        }
    }
}
