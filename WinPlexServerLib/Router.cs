using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace WinPlexServer
{
    public class Router
    {
        Dictionary<string, IController> Controllers = new Dictionary<string,IController>();

        public void IncomingRequest(object sender, HttpRequestEventArgs e)
        {
            HttpListenerResponse response = e.RequestContext.Response;
            HttpListenerRequest httpRequest = e.RequestContext.Request;
            PlexRequest request = new PlexRequest(httpRequest);

            string path = request.AbsolutePath;

            if (request.IsRoot)
            {
                RootIndex(response);
            }
            else
            {
                string controller = path.Split('/')[1];
                if (Controllers.Keys.Contains(controller))
                {
                    Controllers[controller].HandleRequest(request, response);
                }
                else
                {
                    // TODO: 404;
                }
            }
            response.Close();
        }

        public void AddController(string key, IController section)
        {
            Controllers.Add(key, section);
        }

        public void RootIndex(HttpListenerResponse response)
        {
            XmlDocument xml = new XmlDocument();
            XmlElement root = xml.CreateElement("MediaContainer");
            root.SetAttribute("size", Controllers.Count.ToString());
            xml.AppendChild(root);

            foreach (KeyValuePair<string, IController> controller in Controllers)
            {
                XmlElement directory = xml.CreateElement("Directory");
                directory.SetAttribute("count", "1");
                directory.SetAttribute("key", controller.Key);
                directory.SetAttribute("title", controller.Key);
                root.AppendChild(directory);
            }

            string content = xml.OuterXml;
            byte[] buffer = Encoding.UTF8.GetBytes(content);
            response.StatusCode = (int)HttpStatusCode.OK;
            response.StatusDescription = "OK";
            response.ContentType = "text/xml";
            response.ContentEncoding = Encoding.UTF8;
            response.ContentLength64 = buffer.Length;
            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.OutputStream.Close();
            
        }
    }
}
