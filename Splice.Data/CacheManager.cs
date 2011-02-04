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
                string Cache = ConfigurationManager.AppConfigPath + "cache\\";
                if (!Directory.Exists(Cache))
                {
                    Directory.CreateDirectory(Cache);
                }
                return Cache;
            }
        }
        public static string SaveArtwork(Int32 EntityId, string Uri, ArtworkType Type)
        {
            string SavePath = GetCachePath(EntityId, Type);

            string Filename = Path.GetFileName(Uri);
            Uri = BannerRoot + Uri;

            WebClient webClient = new WebClient();
            webClient.DownloadFile(Uri, SavePath + Filename);

            return SavePath + Filename;


        }

        public static string GetCachePath(Int32 EntityId, ArtworkType Type)
        {
            string SavePath = CachePath + EntityId.ToString() + "\\" + Type.ToString() + "\\";
            if (!Directory.Exists(SavePath))
            {
                Directory.CreateDirectory(SavePath);
            }

            return SavePath;
        }

    }
}
