using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Splice.Data;
using System.Xml;

namespace Splice.Server.Controllers
{
    class Manage : IController
    {
        public PlexResponse HandleRequest(PlexRequest Request)
        {
            if (Request.PathSegments.Length > 2)
            {
                if (Request.PathSegments.Length >= 2 && Request.PathSegments[1] == "collections")
                {
                    return ManageCollections(Request);
                }
            }

            return XmlResponse.NotFound();
        }

        private PlexResponse ManageCollections(PlexRequest Request)
        {
            if (Request.PathSegments[2] == "list")
            {
                return CollectionsList();
            }
            else if (Request.PathSegments[2] == "add")
            {
                return AddCollection(Request);
            }
            else if (Request.PathSegments[2] == "delete")
            {
                return DeleteCollection(Convert.ToInt32(Request.PathSegments[3]));
            }
            else
            {
                return XmlResponse.NotFound();
            }
        }

        private PlexResponse DeleteCollection(int CollectionId)
        {
            throw new NotImplementedException();
        }

        private PlexResponse AddCollection(PlexRequest Request)
        {
            if (Request.Method != "POST")
            {
                return XmlResponse.MethodNotAllowed();
            }

            if (Request.PostData["name"] == null || Request.PostData["name"].Length == 0 ||
                Request.PostData["type"] == null)
            {
                // not all required fields provided.
                return XmlResponse.BadRequest();
            }
            else
            {
                VideoCollection Collection = new VideoCollection();

                Collection.Title = Request.PostData["name"];

                try
                {
                    Collection.Type = (VideoCollectionType)Enum.Parse(typeof(VideoCollectionType), Request.PostData["type"]);
                }
                catch (ArgumentException)
                {
                    return XmlResponse.BadRequest();
                }

                // TODO: Handle artwork.

                DataAccess.SaveCollection(Collection);

                // TODO: Return correct response.
                return XmlResponse.NotFound();
            }
        }

        private PlexResponse CollectionsList()
        {
            List<VideoCollection> Collections = DataAccess.GetVideoCollections();

            XmlDocument Doc = new XmlDocument();
            XmlElement CollectionsElement = Doc.CreateElement("Collections");

            Doc.AppendChild(CollectionsElement);

            foreach (VideoCollection Collection in Collections)
            {
                XmlElement CollectionElement = Doc.CreateElement("Collection");
                CollectionElement.SetAttribute("title", Collection.Title);
                CollectionElement.SetAttribute("collectionId", Collection.Id.ToString());
                CollectionElement.SetAttribute("type", Collection.Type.ToString());
                CollectionElement.SetAttribute("art", String.Format("/resources/{0}/art", Collection.Id));

                foreach (string Location in Collection.Locations)
                {
                    XmlElement LocationElement = Doc.CreateElement("Location");
                    LocationElement.SetAttribute("path", Location);
                    CollectionElement.AppendChild(LocationElement);
                }
                CollectionsElement.AppendChild(CollectionElement);
            }

            XmlResponse Response = new XmlResponse();

            Response.XmlDoc = Doc;
            return Response;
        }

    }

}
