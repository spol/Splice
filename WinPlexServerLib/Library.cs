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
            else if (request.PathSegments.Length >= 2 && request.PathSegments[1] == "metadata")
            {
                return MetaData(request);
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
                directory.SetAttribute("title", collection.Title);
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
            List<Filter> filters = Filter.GetTVFilterList();
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
            root.SetAttribute("title1", collection.Title);
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
            VideoCollection currentCollection = DataAccess.GetVideoCollection(collectionId);

            if (currentCollection.Type == "show")
            {
                Filter currentFilter = Filter.GetTVFilter(filterKey);

                List<TVShow> shows = DataAccess.GetTVShows(collectionId, currentFilter);

                XmlDocument xml = new XmlDocument();
                XmlElement root = xml.CreateElement("MediaContainer");
                //size="1"
                root.SetAttribute("size", shows.Count.ToString());
                //mediaTagPrefix="/system/bundle/media/flags/"
                root.SetAttribute("mediaTagPrefix", "/system/bundle/media/flags/");
                //mediaTagVersion="1283229604"
                root.SetAttribute("mediaTagVersion", "1283229604");
                //nocache="1"
                root.SetAttribute("nocache", "1");
                //viewGroup="show"
                root.SetAttribute("viewGroup", "show");
                //viewMode="65592"
                root.SetAttribute("viewMode", "65592");
                //art="/:/resources/show-fanart.jpg"
                root.SetAttribute("art", "/:/resources/show-fanart.jpg");
                //title1="TV Shows"
                root.SetAttribute("title1", "TV Shows");
                //identifier="com.plexapp.plugins.library"
                root.SetAttribute("identifier", "com.plexapp.plugins.library");
                xml.AppendChild(root);

                foreach (TVShow show in shows)
                {
                    root.AppendChild(show.ToXml(xml));
                }

                XmlResponse resp = new XmlResponse();
                resp.XmlDoc = xml;

                return resp;
            }
            else
            {
                return new XmlResponse();
            }
            
        }

        private PlexResponse MetaData(PlexRequest request)
        {
            if (request.PathSegments.Length == 2)
            {
                return XmlResponse.NotFound();
            }
            else if (request.PathSegments.Length == 3)
            {
                return GetMetaData(Convert.ToInt32(request.PathSegments[2]), request);
            }
            else if (request.PathSegments.Length == 4 && request.PathSegments[3] == "children")
            {
                int id = Convert.ToInt32(request.PathSegments[2]);
                return GetMetaDataChildren(id, request);
            }
            else
            {
                return XmlResponse.NotFound();
            }
        }

        private PlexResponse GetMetaData(int id, PlexRequest request)
        {
            return XmlResponse.NotFound();
        }

        private PlexResponse GetMetaDataChildren(int id, PlexRequest request)
        {
            string type = DataAccess.GetType(id);

            if (type == "show")
            {
                return GetShowMetaDataChildren(id);
            }
            else if (type == "season")
            {
                return GetSeasonMetaDataChildren(id);
            }
            else
            {
                return XmlResponse.NotFound();
            }
        }

        private PlexResponse GetSeasonMetaDataChildren(int id)
        {
 	        throw new NotImplementedException();
        }

        private PlexResponse GetShowMetaDataChildren(int id)
        {

            TVShow show = DataAccess.GetTVShow(id);
            List<TVSeason> seasons = DataAccess.GetTVSeasons(show);
            VideoCollection collection = DataAccess.GetVideoCollection(show.Collection);
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("MediaContainer");
            root.SetAttribute("size", seasons.Count.ToString());
            root.SetAttribute("mediaTagPrefix", "/system/bundle/media/flags/");
            root.SetAttribute("mediaTagVersion", "1283229604");
            root.SetAttribute("nocache", "1");
            // Unsure about this one. Possibly position in order from previous level.
            root.SetAttribute("parentIndex", "1");
            root.SetAttribute("parentTitle", show.Title);
            root.SetAttribute("parentYear", show.Year.ToString());
            root.SetAttribute("summary", show.Summary);
            root.SetAttribute("thumb", show.Thumb);
            root.SetAttribute("viewGroup", "seasons");
            root.SetAttribute("viewMode", "65593");
            root.SetAttribute("key", show.Id.ToString());
            root.SetAttribute("art", show.Art);
            root.SetAttribute("title1", collection.Title);
            root.SetAttribute("title2", show.Title);
            root.SetAttribute("identifier", "com.plexapp.plugins.library");
            doc.AppendChild(root);

            foreach (TVSeason season in seasons)
            {
                XmlElement el = doc.CreateElement("Directory");

                el.SetAttribute("ratingKey", season.Id.ToString());
                el.SetAttribute("key", String.Format("/library/metadata/{0}/children", season.Id));
                el.SetAttribute("type", "season");
                el.SetAttribute("title", season.Title);
                el.SetAttribute("summary", "");
                el.SetAttribute("index", "1");
                el.SetAttribute("thumb", "");
                el.SetAttribute("leafCount", "0");
                el.SetAttribute("viewedLeafCount", "0");

                root.AppendChild(el);
            }
            XmlResponse resp = new XmlResponse();
            resp.XmlDoc = doc;
            return resp;
        }

    }
}
