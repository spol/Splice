using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace WinPlexServer
{
    public class Library : IController
    {
        public void HandleRequest(PlexRequest request, HttpListenerResponse response)
        {
            if (request.PathSegments.Length == 1)
            {
                Index(request, response);
            }
            else if (request.PathSegments[1] == "sections")
            {
                Sections(request, response);
            }
            else
            {
                string path = request.ControllerPath;
                string content = ""; //path;
                byte[] buffer = Encoding.UTF8.GetBytes(content);
                response.StatusCode = (int)HttpStatusCode.OK;
                response.StatusDescription = "OK";
                response.ContentLength64 = buffer.Length;
                response.OutputStream.Write(buffer, 0, buffer.Length);
                response.OutputStream.Close();
            }
        }

        public void Index(PlexRequest request, HttpListenerResponse response)
        {
            XmlDocument xml = new XmlDocument();
            XmlElement root = xml.CreateElement("MediaContainer");
            root.SetAttribute("size", "1");
            root.SetAttribute("mediaTagPrefix", "/system/bundle/media/flags");
            root.SetAttribute("mediaTagVersion", "1283229604");
            root.SetAttribute("art", "/:/resources/library-art.png");
            root.SetAttribute("title1", "WinPlex Library");
            root.SetAttribute("identify", "com.plexapp.plugins.library");
            xml.AppendChild(root);

            XmlElement directory = xml.CreateElement("Directory");
            directory.SetAttribute("key", "sections");
            directory.SetAttribute("title", "Library Sections");
            root.AppendChild(directory);

            XmlResponse xmlResponse = new XmlResponse(response);
            xmlResponse.XmlDoc = xml;
            xmlResponse.Send();

        }

        public void Sections(PlexRequest request, HttpListenerResponse response)
        {

            XmlDocument xml = new XmlDocument();
            XmlDeclaration dec = xml.CreateXmlDeclaration("1.0", "UTF-8", null);
            xml.AppendChild(dec);
            XmlElement root = xml.CreateElement("MediaContainer");
            root.SetAttribute("size", "1");
            xml.AppendChild(root);


            //foreach (KeyValuePair<string, IController> controller in Controllers)
            //{
            XmlElement directory = xml.CreateElement("Directory");
            directory.SetAttribute("key", "2");
            directory.SetAttribute("type", "show");
            directory.SetAttribute("title", "WinPlex");
            directory.SetAttribute("art", "/:/resources/show-fanart.jpg");
            root.AppendChild(directory);
            //}

            XmlResponse xmlResponse = new XmlResponse(response);
            xmlResponse.XmlDoc = xml;
            xmlResponse.Send();

        }
    }
}
