using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Splice.Server
{
    public class PlexRequest
    {
        public string AbsolutePath
        {
            get
            {
                if (Request.Url.AbsolutePath.EndsWith("/"))
                {

                    return Request.Url.AbsolutePath;
                }
                else
                {
                    return Request.Url.AbsolutePath + "/";
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
                return Request.Headers;
            }
        }

        public String Method
        {
            get
            {
                return Request.HttpMethod;
            }
        }

        private PostData _PostData;
        public PostData PostData
        {
            get
            {
                if (_PostData == null)
                {
                    ParsePostData();
                }
                return _PostData;
            }
        }

        private void ParsePostData()
        {
            if (!Request.HasEntityBody)
            {
                return;
            }
            else
            {
                if (Request.ContentType.StartsWith("multipart/form-data;"))
                {
                    _PostData = new MultipartPostData(Request);
                }
                else
                {
                    _PostData = new UrlEncodedPostData(Request);
                }
            }
        }

        private HttpListenerRequest Request;


        public PlexRequest(HttpListenerRequest Req)
        {

            Request = Req;
        }
    }
}
