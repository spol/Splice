using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using Splice.Data;
using Splice.Data.Filters;

namespace Splice.Server
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
            else if (request.PathSegments.Length >= 2 && request.PathSegments[1] == "parts")
            {
                return Parts(request);
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
            root.SetAttribute("art", "/resources/library-art.png");
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
                directory.SetAttribute("type", collection.Type.ToString());
                directory.SetAttribute("title", collection.Title);
                directory.SetAttribute("art", String.Format("/resources/{0}/art", collection.Id));
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
            root.SetAttribute("art", String.Format("/resources/{0}/art", collection.Id));
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

            if (currentCollection.Type == VideoCollectionType.show)
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
                root.SetAttribute("art", "/resources/show-fanart.jpg");
                //title1="TV Shows"
                root.SetAttribute("title1", "TV Shows");
                //identifier="com.plexapp.plugins.library"
                root.SetAttribute("identifier", "com.plexapp.plugins.library");
                xml.AppendChild(root);

                foreach (TVShow show in shows)
                {
                    XmlElement el = xml.CreateElement("Directory");
                    el.SetAttribute("ratingKey", show.Id.ToString());
                    el.SetAttribute("key", String.Format("/library/metadata/{0}/children", show.Id));
                    el.SetAttribute("studio", show.Studio);
                    el.SetAttribute("type", show.Type);
                    el.SetAttribute("title", show.Title);
                    el.SetAttribute("contentRating", show.ContentRating);
                    el.SetAttribute("summary", show.Summary);
                    el.SetAttribute("rating", show.Rating.ToString());
                    el.SetAttribute("year", show.Year.ToString());
                    el.SetAttribute("thumb", String.Format("/resources/{0}/thumb/{1}", show.Id, show.LastUpdated));
                    el.SetAttribute("art", String.Format("/resources/{0}/art/{1}", show.Id, show.LastUpdated));
                    el.SetAttribute("banner", String.Format("/resources/{0}/banner/{1}", show.Id, show.LastUpdated));
                    el.SetAttribute("duration", show.Duration.ToString());
                    el.SetAttribute("originallyAvailableAt", show.AirDate.ToShortDateString());
                    el.SetAttribute("leafCount", show.LeafCount.ToString());
                    el.SetAttribute("viewedLeafCount", show.ViewedLeafCount.ToString());
                    root.AppendChild(el);
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

        private PlexResponse GetMetaDataChildren(int Id, PlexRequest request)
        {
            SpliceEntityType Type = DataAccess.GetEntityType(Id);

            if (Type == SpliceEntityType.TVShow)
            {
                return GetShowMetaDataChildren(Id);
            }
            else if (Type == SpliceEntityType.TVSeason)
            {
                return GetSeasonMetaDataChildren(Id);
            }
            else
            {
                return XmlResponse.NotFound();
            }
        }

        private PlexResponse GetSeasonMetaDataChildren(int SeasonId)
        {
            TVSeason Season = DataAccess.GetTVSeason(SeasonId);
            TVShow Show = DataAccess.GetTVShow(Season.ShowId);
            List<TVEpisode> Episodes = DataAccess.GetTVEpisodes(Season);

            XmlDocument doc = new XmlDocument();
            XmlElement MediaContainerElement = doc.CreateElement("MediaContainer");
            MediaContainerElement.SetAttribute("size", Episodes.Count.ToString());
            MediaContainerElement.SetAttribute("grandparentContentRating", Show.ContentRating);
            MediaContainerElement.SetAttribute("grandparentStudio", Show.Studio);
            MediaContainerElement.SetAttribute("grandparentTitle", Show.Title);
            MediaContainerElement.SetAttribute("mediaTagPrefix", "/system/bundle/media/flags/");
            // TODO: Currently static
            MediaContainerElement.SetAttribute("mediaTagVersion", "1283229604");
            MediaContainerElement.SetAttribute("nocache", "1");
            // TODO: Unsure about this one. Possibly position in order from previous level.
            MediaContainerElement.SetAttribute("parentIndex", "1");
            MediaContainerElement.SetAttribute("parentTitle", "");
            MediaContainerElement.SetAttribute("thumb", String.Format("/resources/{0}/thumb", Show.Id));
            MediaContainerElement.SetAttribute("viewGroup", "episode");
            // TODO: Currently static
            MediaContainerElement.SetAttribute("viewMode", "65592");
            MediaContainerElement.SetAttribute("key", Season.Id.ToString());
            MediaContainerElement.SetAttribute("banner", String.Format("/resources/{0}/banner", Show.Id));
            MediaContainerElement.SetAttribute("art", String.Format("/resources/{0}/art", Show.Id));
            // TODO: Fetch theme;
            MediaContainerElement.SetAttribute("theme", "");
            MediaContainerElement.SetAttribute("title1", Show.Title);
            MediaContainerElement.SetAttribute("title2", Season.Title);
            MediaContainerElement.SetAttribute("identifier", "com.plexapp.plugins.library");
            doc.AppendChild(MediaContainerElement);

            foreach (TVEpisode Episode in Episodes)
            {
                XmlElement VideoElement = doc.CreateElement("Video");

                VideoElement.SetAttribute("ratingKey", Episode.Id.ToString());
                VideoElement.SetAttribute("key", String.Format("/library/metadata/{0}", Episode.Id));
                VideoElement.SetAttribute("type", Episode.Type);
                VideoElement.SetAttribute("title", Episode.Title);
                VideoElement.SetAttribute("summary", Episode.Summary);
                VideoElement.SetAttribute("index", Episode.EpisodeNumber.ToString());
                VideoElement.SetAttribute("rating", Episode.Rating.ToString("F1"));
                VideoElement.SetAttribute("thumb", String.Format("/resources/{0}/thumb/{1}", Episode.Id, Episode.LastUpdated));
                VideoElement.SetAttribute("duration", Episode.Duration.ToString());
                VideoElement.SetAttribute("originallyAvailableAt", Episode.AirDate.ToString("yyyy-MM-dd"));

                // TODO Add Media / Writer / Director tags.
                List<VideoFileInfo> VideoFiles = DataAccess.GetVideoFilesForEpisode(Episode.Id);

                foreach (VideoFileInfo VideoFile in VideoFiles)
                {

                    XmlElement MediaElement = doc.CreateElement("Media");
                    MediaElement.SetAttribute("id", VideoFile.Id.ToString());
                    MediaElement.SetAttribute("duration", VideoFile.Duration.ToString());
                    MediaElement.SetAttribute("bitrate", VideoFile.Bitrate.ToString());
                    MediaElement.SetAttribute("aspectRatio", VideoFile.AspectRatio.ToString());
                    MediaElement.SetAttribute("audioChannels", VideoFile.AudioChannels.ToString());
                    MediaElement.SetAttribute("audioCodec", VideoFile.AudioCodec);
                    MediaElement.SetAttribute("videoCodec", VideoFile.VideoCodec);
                    MediaElement.SetAttribute("videoResolution", String.Format("{0}x{1}", VideoFile.PictureWidth, VideoFile.PictureHeight));
                    MediaElement.SetAttribute("videoFrameRate", VideoFile.FrameRate.ToString());

                    XmlElement PartElement = doc.CreateElement("Part");
                    PartElement.SetAttribute("key", String.Format("/library/parts/{0}", VideoFile.Id));
                    PartElement.SetAttribute("file", VideoFile.Path);
                    PartElement.SetAttribute("size", VideoFile.Size.ToString());

                    MediaElement.AppendChild(PartElement);
                    VideoElement.AppendChild(MediaElement);
                }
                MediaContainerElement.AppendChild(VideoElement);
            }
            XmlResponse resp = new XmlResponse();
            resp.XmlDoc = doc;
            return resp;
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
            root.SetAttribute("art", String.Format("/resources/{0}/art", show.Id));
            root.SetAttribute("title1", collection.Title);
            root.SetAttribute("title2", show.Title);
            root.SetAttribute("identifier", "com.plexapp.plugins.library");
            doc.AppendChild(root);

            foreach (TVSeason Season in seasons)
            {
                XmlElement el = doc.CreateElement("Directory");

                el.SetAttribute("ratingKey", Season.Id.ToString());
                el.SetAttribute("key", String.Format("/library/metadata/{0}/children", Season.Id));
                el.SetAttribute("type", "season");
                el.SetAttribute("title", Season.Title);
                el.SetAttribute("summary", "");
                el.SetAttribute("index", "1");
                el.SetAttribute("thumb", String.Format("/resources/{0}/thumb", Season.Id));
                el.SetAttribute("leafCount", "0");
                el.SetAttribute("viewedLeafCount", "0");

                root.AppendChild(el);
            }
            XmlResponse resp = new XmlResponse();
            resp.XmlDoc = doc;
            return resp;
        }

        private PlexResponse Parts(PlexRequest request)
        {
            if (request.PathSegments.Length < 3)
            {
                return XmlResponse.NotFound();
            }
            else
            {
                int PartId = Convert.ToInt32(request.PathSegments[2]);

                VideoFileInfo vidFile = DataAccess.GetVideoFile(PartId);

                VideoResponse resp = new VideoResponse();
                resp.FilePath = vidFile.Path;

                if (request.Headers["Range"] != null)
                {
                    string[] range = request.Headers["Range"].Substring(6).Split('-');
                    resp.Start = Convert.ToInt64(range[0]);
                    if (range[1] != "")
                    {
                        resp.End = Convert.ToInt64(range[1]);
                    }
                }

                return resp;
            }
        }

    }
}
