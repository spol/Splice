using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace WinPlexServer
{
    public class Router
    {
        public void IncomingRequest(object sender, HttpRequestEventArgs e)
        {
            //e.RequestContext.Request.Url;
            //e.RequestContext.Request.QueryString;
            HttpListenerResponse response = e.RequestContext.Response;
            string content = e.RequestContext.Request.Url.AbsolutePath;
            byte[] buffer = Encoding.UTF8.GetBytes(content);
            response.StatusCode = (int)HttpStatusCode.OK;
            response.StatusDescription = "OK";
            response.ContentLength64 = buffer.Length;
            response.ContentEncoding = Encoding.UTF8;
            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.OutputStream.Close();
            response.Close();
        }
    }
}
