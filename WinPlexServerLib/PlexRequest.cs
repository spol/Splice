using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace WinPlexServer
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

        private HttpListenerRequest request;


        public PlexRequest(HttpListenerRequest req)
        {

            request = req;
        }
    }
}
