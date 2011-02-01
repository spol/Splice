using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml;
using Splice.Data;

namespace Splice.Manager
{
    class Service
    {
        public static List<VideoCollection> GetCollectionsList()
        {
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create("http://localhost:32400/manage/collections/list");
            HttpWebResponse Response = (HttpWebResponse)Request.GetResponse();

            XmlTextReader XmlReader = new XmlTextReader(Response.GetResponseStream());

            while (XmlReader.NodeType != XmlNodeType.Element && XmlReader.Name != "Collections" && !XmlReader.EOF)
            {
                XmlReader.Read();
            }

            List<VideoCollection> Collections = new List<VideoCollection>();
            VideoCollection Collection = null;

            if (!XmlReader.EOF && XmlReader.Name == "Collections")
            {
                while (!XmlReader.EOF)
                {
                    //DebugOutput.Text += XmlReader.Name;
                    XmlReader.Read();
                    if (XmlReader.Name == "Collection" && XmlReader.NodeType == XmlNodeType.Element)
                    {
                        Collection = new VideoCollection();
                        Collection.Title = XmlReader["title"];
                        Collection.Id = Convert.ToInt32(XmlReader["collectionId"]);
                        Collection.Art = XmlReader["art"];
                        Collection.Type = (VideoCollectionType)Enum.Parse(typeof(VideoCollectionType), XmlReader["type"]);

                        XmlReader.Read();
                        while (XmlReader.NodeType != XmlNodeType.EndElement && XmlReader.Name != "Collection" && XmlReader.EOF)
                        {
                            if (XmlReader.Name == "Location")
                            {
                                Collection.Locations.Add(XmlReader["path"]);
                            }
                            XmlReader.Read();
                        }
                        Collections.Add(Collection);
                        Collection = null;
                    }

                    if (XmlReader.Name == "Collections" && XmlReader.NodeType == XmlNodeType.EndElement)
                    {
                        break;
                    }
                }

                return Collections;
            }

            throw new Exception("Collections element not found.");

        }
    }
}
