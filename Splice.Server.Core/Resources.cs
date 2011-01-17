using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Splice.Data;

namespace Splice.Server
{
    class Resources : IController
    {
        public PlexResponse HandleRequest(PlexRequest Request)
        {
            if (Request.PathSegments.Length <= 2)
            {
                return XmlResponse.NotFound();
            }
            else
            {
                Int32 Id = Convert.ToInt32(Request.PathSegments[1]);
                SpliceEntityType Type = DataAccess.GetEntityType(Id);

                switch (Type)
                {
                    case SpliceEntityType.Collection:
                        return HandleCollectionResourceRequest(Id, Request);
                    case SpliceEntityType.TVShow:
                        return HandleTVShowResourceRequest(Id, Request);
                    default:
                        return XmlResponse.NotFound();
                }
            }
        }

        private PlexResponse HandleTVShowResourceRequest(int Id, PlexRequest Request)
        {
            TVShow Show = DataAccess.GetTVShow(Id);

            ImageResponse Response = new ImageResponse();
            switch (Request.PathSegments[2])
            {
                case "art":
                    Response.FilePath = Show.Art;
                    return Response;
                case "thumb":
                    Response.FilePath = Show.Thumb;
                    return Response;
                case "banner":
                    Response.FilePath = Show.Banner;
                    return Response;
                default:
                    return XmlResponse.NotFound();
            }

        }

        private PlexResponse HandleCollectionResourceRequest(Int32 Id, PlexRequest Request)
        {
            VideoCollection Collection = DataAccess.GetVideoCollection(Id);

            switch (Request.PathSegments[2])
            {
                case "art":
                    ImageResponse Response = new ImageResponse();
                    Response.FilePath = Collection.Art;
                    return Response;
                default:
                    return XmlResponse.NotFound();
            }
        }

    }
}
