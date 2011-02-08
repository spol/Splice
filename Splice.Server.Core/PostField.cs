using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

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

        protected Byte[] _Data;
        public Byte[] Data
        {
            get
            {
                return _Data;
            }
        }
        public String DataAsString
        {
            get
            {
                return Encoding.UTF8.GetString(_Data);
            }
        }

        private NameValueCollection _Headers = new NameValueCollection();
        public NameValueCollection Headers
        {
            get
            {
                return _Headers;
            }
        }

        private Boolean _IsFile;
        public Boolean IsFile
        {
            get
            {
                return _IsFile;
            }
        }

        public PostField(Byte[] BinaryData)
        {
            // Find \r\n\r\n
            for (Int32 i = 0; i < BinaryData.Length; i++)
            {
                if (BinaryData[i] == (Byte)'\r' && Encoding.UTF8.GetString(BinaryData, i, 4) == "\r\n\r\n")
                {
                    // boundary found
                    ProcessHeaders(Encoding.UTF8.GetString(BinaryData, 0, i));

                    Int64 Offset = i + 4;
                    Int64 DataLength = BinaryData.Length - Offset;

                    _Data = new Byte[DataLength];

                    Array.Copy(BinaryData, Offset, _Data, 0, DataLength);
                    return;
                }
            }

            throw new Exception("Header separator not found.");
        }

        public PostField(String Name, String Field)
        {
            _Name = Name;
            _Data = Encoding.UTF8.GetBytes(Field);
        }

        private void ProcessHeaders(string Headers)
        {
            String[] HeaderLines = Headers.Split(new String[] { "\r\n" }, StringSplitOptions.None);

            foreach (String Header in HeaderLines)
            {
                String[] Parts = Header.Split(new Char[] { ':' }, 2);
                _Headers.Add(Parts[0], Parts[1]);

                // check for name
                if (Parts[0] == "Content-Disposition")
                {
                    Regex R = new Regex("name=\"(.*?)\"");

                    Match M = R.Match(Parts[1]);

                    if (M.Groups.Count > 1)
                    {
                        _Name = M.Groups[1].Value;
                    }

                    Regex R2 = new Regex("filename=\"(.*?)\"");

                    M = R2.Match(Parts[1]);

                    if (M.Groups.Count > 1)
                    {
                        _IsFile = true;
                    }
                }
            }
        }
    }
}
