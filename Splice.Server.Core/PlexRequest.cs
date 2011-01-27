using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Splice.Server
{
    public class PlexRequest
    {
        public string AbsolutePath
        {
            get
            {
                if (request.Url.AbsolutePath.EndsWith("/"))
                {

                    return request.Url.AbsolutePath;
                }
                else
                {
                    return request.Url.AbsolutePath + "/";
                }
            }
        }

        public string[] PathSegments
        {
            get
            {
                return AbsolutePath.Trim('/').Split('/');
            }
        }

        public bool IsRoot
        {
            get
            {
                return AbsolutePath == "/";
            }
        }

        public string Controller
        {
            get
            {
                if (IsRoot)
                {
                    return null;
                }
                else
                {
                    return AbsolutePath.Split('/')[1];
                }
            }
        }

        public string ControllerPath
        {
            get
            {
                return AbsolutePath.Substring(Controller.Length + 1);
            }
        }

        public NameValueCollection Headers
        {
            get
            {
                return request.Headers;
            }
        }

        public String Method
        {
            get
            {
                return request.HttpMethod;
            }
        }

        public NameValueCollection PostData
        {
            get
            {
                if (!request.HasEntityBody)
                {
                    return null;
                }
                else
                {
                    StreamReader Reader = new StreamReader(request.InputStream);

                    String Data = Reader.ReadToEnd();
                    NameValueCollection Fields = new NameValueCollection();
                    String[] KeyValues = Data.Split('&');
                    foreach (String KeyValue in KeyValues)
                    {
                        String[] Parts = KeyValue.Split('=');
                        Fields.Add(Parts[0], Parts[1]);
                    }
                    return Fields;
                }
            }
        }

        private HttpListenerRequest request;


        public PlexRequest(HttpListenerRequest req)
        {

            request = req;
        }
    }
}
