using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml;
using Splice.Data;
using Krystalware.UploadHelper;
using System.Collections.Specialized;
using System.Web;

namespace Splice.Manager
{
    class Service
    {
        public static List<VideoCollection> GetCollectionsList()
        {
            HttpWebResponse Response = null;
            try
            {
                HttpWebRequest Request = (HttpWebRequest)WebRequest.Create("http://localhost:32400/manage/collections/list");
                Request.KeepAlive = false;
                Response = (HttpWebResponse)Request.GetResponse();

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
                        XmlReader.Read();
                        if (XmlReader.Name == "Collection" && XmlReader.NodeType == XmlNodeType.Element)
                        {
                            Collection = new VideoCollection();
                            Collection.Title = XmlReader["title"];
                            Collection.Id = Convert.ToInt32(XmlReader["collectionId"]);
                            Collection.Art = XmlReader["art"];
                            Collection.Type = (VideoCollectionType)Enum.Parse(typeof(VideoCollectionType), XmlReader["type"]);

                            if (!XmlReader.IsEmptyElement)
                            {

                                XmlReader.Read();
                                while (XmlReader.NodeType != XmlNodeType.EndElement && XmlReader.Name != "Collection" && XmlReader.EOF)
                                {
                                    if (XmlReader.Name == "Location")
                                    {
                                        Collection.Locations.Add(XmlReader["path"]);
                                    }
                                    XmlReader.Read();
                                }
                            }
                            Collections.Add(Collection);
                            Collection = null;
                        }

                        if (XmlReader.Name == "Collections" && XmlReader.NodeType == XmlNodeType.EndElement)
                        {
                            break;
                        }
                    }
                    Response.Close();
                    return Collections;
                }

                throw new Exception("Collections element not found.");
            }
            catch (WebException)
            {
                // TODO: Handle unable to connect.
            }
            finally
            {
                if (Response != null)
                {
                    Response.Close();
                }
            }
            return new List<VideoCollection>();

        }

        internal static void CreateCollection(Collection Collection)
        {
            Stream FileStream = new MemoryStream();

            Collection.Artwork.Save(FileStream, System.Drawing.Imaging.ImageFormat.Jpeg);

            FileStream.Seek(0, SeekOrigin.Begin);

            UploadFile Artwork = new UploadFile(FileStream, "Artwork", "artwork.jpg", "image/jpg");

            UploadFile[] Files = new UploadFile[1];
            Files[0] = Artwork;

            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create("http://localhost:32400/manage/collections/add");
            Request.Proxy = new WebProxy("localhost", 8888);
            Request.ServicePoint.Expect100Continue = false;
            Request.KeepAlive = false;
            Request.Timeout = 1000 * 60 * 10;
            HttpWebResponse Response = HttpUploadHelper.Upload(Request, Files, Collection.ToNameValueCollection()); 

        }

        public static void DeleteCollection(Int32 CollectionId)
        {
            NameValueCollection Fields = new NameValueCollection();
            Fields.Add("CollectionId", CollectionId.ToString());

            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create("http://localhost:32400/manage/collections/delete");
            Request.Method = "POST";
            Request.ContentType = "application/x-www-form-urlencoded";
            Stream RequestStream = Request.GetRequestStream();

            Byte[] RequestData = Encoding.UTF8.GetBytes(UrlEncodeForm(Fields));

            RequestStream.Write(RequestData, 0, RequestData.Length);
            RequestStream.Close();

            HttpWebResponse Response = (HttpWebResponse)Request.GetResponse();
        }

        private static String UrlEncodeForm(NameValueCollection Fields)
        {
            StringBuilder Builder = new StringBuilder();

            foreach (String FieldName in Fields.AllKeys)
            {
                if (Builder.Length != 0)
                {
                    Builder.Append("&");
                }
                Builder.Append(HttpUtility.UrlEncode(FieldName) + "=" + HttpUtility.UrlEncode(Fields[FieldName]));
            }

            return Builder.ToString();
        }
    }
}
