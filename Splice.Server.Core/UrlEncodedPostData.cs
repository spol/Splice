using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;
using System.IO;
using System.Collections.Specialized;

namespace Splice.Server
{
    class UrlEncodedPostData : PostData
    {
        public UrlEncodedPostData(HttpListenerRequest Request)
        {
            if (!Request.ContentType.StartsWith("application/x-www-form-urlencoded"))
            {
                throw new Exception("Not a Url Encoded Post Request");
            }

            MemoryStream MemStream = new MemoryStream();
            Byte[] BinaryData = new Byte[512];
            int ReadCount = 0;
            while ((ReadCount = Request.InputStream.Read(BinaryData, 0, BinaryData.Length)) > 0)
            {
                MemStream.Write(BinaryData, 0, ReadCount);
            }

            NameValueCollection Query = HttpUtility.ParseQueryString(Encoding.UTF8.GetString(MemStream.ToArray()), Encoding.UTF8);

            foreach (String Key in Query.AllKeys)
            {
                Fields.Add(Key, new PostField(Key, Query[Key]));
            }
        }

    }
}
