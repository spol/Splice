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
            PlexResponse resp;
            if (request.IsRoot)
            {
                resp = RootIndex();
            }
            else
            {
                string controller = path.Split('/')[1];
                if (Controllers.Keys.Contains(controller))
                {
                    resp = Controllers[controller].HandleRequest(request);
                }
                else
                {
                    // TODO: 404;
                    resp = XmlResponse.NotFound();
                }
            }
            resp.Send(response);
//            response.Close();
        }

        public void AddController(string key, IController section)
        {
            Controllers.Add(key, section);
        }

        public PlexResponse RootIndex()
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

            XmlResponse response = new XmlResponse();
            response.XmlDoc = xml;
            return response;          
        }
    }
}
