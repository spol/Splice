using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Splice.Configuration;

namespace Splice.Data
{
    public class CacheManager
    {
        private static string BannerRoot = "http://www.thetvdb.com/banners/";
        public static string CachePath
        {
            get
            {
                string Cache = ConfigurationManager.AppConfigPath + "/cache/";
                if (!Directory.Exists(Cache))
                {
                    Directory.CreateDirectory(Cache);
                }
                return Cache;
            }
        }
        public static string SaveArtwork(Int32 EntityId, string Uri)
        {
            string EntityPath = CachePath + EntityId.ToString() + "/";
            if (!Directory.Exists(EntityPath))
            {
                Directory.CreateDirectory(EntityPath);
            }

            string Filename = Path.GetFileName(Uri);
            WebRequest Request = WebRequest.Create(BannerRoot + Uri);
            WebResponse Response = Request.GetResponse();
            Stream Stream = Response.GetResponseStream();

            FileStream F = new FileStream(EntityPath + Filename, FileMode.CreateNew);

            BinaryWriter Writer = new BinaryWriter(F);
            StreamReader Reader = new StreamReader(Stream);
            Writer.Write(Reader.ReadToEnd());
            return EntityPath + Filename;
        }
    }
}
