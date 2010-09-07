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
            else if (request.PathSegments.Length == 2 && request.PathSegments[1] == "sections")
            {
                SectionsIndex(request, response);
            }
            else if (request.PathSegments.Length == 3 && request.PathSegments[1] == "sections")
            {
                SectionListing(Convert.ToInt32(request.PathSegments[2]), request, response);
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

        public void SectionsIndex(PlexRequest request, HttpListenerResponse response)
        {
            XmlDocument xml = new XmlDocument();
            XmlDeclaration dec = xml.CreateXmlDeclaration("1.0", "UTF-8", null);
            xml.AppendChild(dec);
            XmlElement root = xml.CreateElement("MediaContainer");
            List<VideoCollection> collections = DataAccess.GetVideoCollections();
            root.SetAttribute("size", collections.Count);
            xml.AppendChild(root);

            foreach (VideoCollection collection in collections)
            {
                XmlElement directory = xml.CreateElement("Directory");
                directory.SetAttribute("key", collection.Id.ToString());
                directory.SetAttribute("type", collection.Type);
                directory.SetAttribute("title", collection.Name);
                directory.SetAttribute("art", collection.Art);
                root.AppendChild(directory);
            }

            XmlResponse xmlResponse = new XmlResponse(response);
            xmlResponse.XmlDoc = xml;
            xmlResponse.Send();
        }

        public void SectionListing(int sectionId, PlexRequest request, HttpListenerResponse response)
        {
            VideoCollection collection = DataAccess.GetCollection(sectionId);
            XmlDocument xml = new XmlDocument();
            XmlDeclaration dec = xml.CreateXmlDeclaration("1.0", "UTF-8", null);
            xml.AppendChild(dec);
            XmlElement root = xml.CreateElement("MediaContainer");
            //size="11" 
            root.SetAttribute("size", "11");
            //content="secondary" 
            root.SetAttribute("content", "secondary");
            //mediaTagPrefix="/system/bundle/media/flags/" 
            root.SetAttribute("mediaTagPrefix", "/system/bundle/media/flags/");
            //mediaTagVersion="1283229604" 
            root.SetAttribute("mediaTagVersion", "1283229604");
            //nocache="1" 
            root.SetAttribute("nocache", "1");
            //viewGroup="secondary" 
            root.SetAttribute("viewGroup", "secondary");
            //viewMode="65592" 
            root.SetAttribute("viewMode", "65592");
            //art="/:/resources/show-fanart.jpg" 
            root.SetAttribute("art", "/:/resources/show-fanart.jpg");
            //identifier="com.plexapp.plugins.library"
            root.SetAttribute("identifier", "com.plexapps.plugins.library");
            //title1="TV Shows" 
            root.SetAttribute("title1", "11");
            xml.AppendChild(root);
        }
    }
}
