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
        public PlexResponse HandleRequest(PlexRequest request)
        {
            if (request.PathSegments.Length == 1)
            {
                return Index(request);
            }
            else if (request.PathSegments.Length >= 2 && request.PathSegments[1] == "sections")
            {
                return Sections(request);
            }
            else
            {
                return XmlResponse.NotFound();
            }
        }

        public PlexResponse Index(PlexRequest request)
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

            XmlResponse xmlResponse = new XmlResponse();
            xmlResponse.XmlDoc = xml;
            return xmlResponse;
        }

        public PlexResponse Sections(PlexRequest request)
        {
            if (request.PathSegments.Length == 2)
            {
                return SectionsIndex(request);
            }
            else if (request.PathSegments.Length == 3)
            {
                return SectionListing(Convert.ToInt32(request.PathSegments[2]), request);
            }
            else if (request.PathSegments.Length == 4)
            {
                int collectionId = Convert.ToInt32(request.PathSegments[2]);
                string filterKey = request.PathSegments[3];
                return FilteredSection(collectionId, filterKey);
            }
            else
            {
                return XmlResponse.NotFound();
            }
        }

        public PlexResponse SectionsIndex(PlexRequest request)
        {
            XmlDocument xml = new XmlDocument();
            XmlDeclaration dec = xml.CreateXmlDeclaration("1.0", "UTF-8", null);
            xml.AppendChild(dec);
            XmlElement root = xml.CreateElement("MediaContainer");
            List<VideoCollection> collections = DataAccess.GetVideoCollections();
            root.SetAttribute("size", collections.Count.ToString());
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

            XmlResponse xmlResponse = new XmlResponse();
            xmlResponse.XmlDoc = xml;
            return xmlResponse;
        }

        public PlexResponse SectionListing(int sectionId, PlexRequest request)
        {
            VideoCollection collection = DataAccess.GetVideoCollection(sectionId);
            List<Filter> filters = Filter.GetList();
            XmlDocument xml = new XmlDocument();
            XmlDeclaration dec = xml.CreateXmlDeclaration("1.0", "UTF-8", null);
            xml.AppendChild(dec);
            XmlElement root = xml.CreateElement("MediaContainer");
            //size="11" 
            root.SetAttribute("size", filters.Count.ToString());
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
            root.SetAttribute("title1", collection.Name);
            xml.AppendChild(root);

            foreach (Filter filter in filters)
            {
                XmlElement directory = xml.CreateElement("Directory");
                directory.SetAttribute("key", filter.Key);
                directory.SetAttribute("title", filter.Name);
                root.AppendChild(directory);
            }

            XmlResponse xmlResponse = new XmlResponse();
            xmlResponse.XmlDoc = xml;
            return xmlResponse;
        }

        private PlexResponse FilteredSection(int collectionId, string filterKey)
        {
            throw new NotImplementedException();
        }
    }
}
