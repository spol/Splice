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
        Dictionary<string, Section> Sections = new Dictionary<string,Section>();

        public void IncomingRequest(object sender, HttpRequestEventArgs e)
        {
            Uri Url = e.RequestContext.Request.Url;
            string path = e.RequestContext.Request.Url.AbsolutePath;

            if (!path.EndsWith("/"))
            {
                path = path += "/";
            }
            
            //e.RequestContext.Request.QueryString;
            HttpListenerResponse response = e.RequestContext.Response;
            response.ContentEncoding = Encoding.UTF8;
            if (path == "/")
            {
                RootIndex(response);
            }
            else
            {
                string section = path.Split('/')[1];
                if (Sections.Keys.Contains(section))
                {
                    path = path.Substring(section.Length + 1);
                    Sections[section].HandleRequest(response, path);
                }
            }
            response.Close();
        }

        public void AddSection(string key, Section section)
        {
            Sections.Add(key, section);
        }

        public void RootIndex(HttpListenerResponse response)
        {
            XmlDocument xml = new XmlDocument();
            XmlElement root = xml.CreateElement("MediaContainer");
            root.SetAttribute("size", Sections.Count.ToString());
            xml.AppendChild(root);

            foreach (KeyValuePair<string, Section> section in Sections)
            {
                XmlElement directory = xml.CreateElement("Directory");
                directory.SetAttribute("count", "1");
                directory.SetAttribute("key", section.Key);
                directory.SetAttribute("title", section.Key);
                root.AppendChild(directory);
            }

            string content = xml.OuterXml;
            byte[] buffer = Encoding.UTF8.GetBytes(content);
            response.StatusCode = (int)HttpStatusCode.OK;
            response.StatusDescription = "OK";
            response.ContentLength64 = buffer.Length;
            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.OutputStream.Close();
            
        }
    }
}
