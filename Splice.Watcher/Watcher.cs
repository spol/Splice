using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Splice.Data;

namespace SpliceWatcher
{
    public class Watcher
    {
        public void ScanAll()
        {
            List<VideoCollection> collections = DataAccess.GetVideoCollections();

            foreach (VideoCollection collection in collections)
            {
                ScanCollection(collection);
            }
        }

        public void ScanCollection(VideoCollection collection)
        {
            switch (collection.Type)
            {
                case "show":
                    ScanTVCollection(collection);
                    break;
                case "movie":
                    ScanMovieCollection(collection);
                    break;
            }
        }

        private void ScanMovieCollection(VideoCollection collection)
        {
            throw new NotImplementedException();
        }

        private void ScanTVCollection(VideoCollection collection)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(collection.Root);

            DirectoryInfo[] showDirectories = dirInfo.GetDirectories();

            foreach (DirectoryInfo showDir in showDirectories)
            {
                TVShow show = DataAccess.GetTVShowFromPath(showDir.FullName);
                if (show != null)
                {
                    // Show already in DB
                    UpdateShow(show);
                }
                else
                {
                    // New Show.
                    LookupShow(showDir.Name);
                }
            }
        }

        private void UpdateShow(TVShow show)
        {
            DirectoryInfo showDir = new DirectoryInfo(show.Location);
            FileInfo[] files = showDir.GetFiles();

            foreach (FileInfo file in files)
            {
            }
        }

        private void LookupShow(string p)
        {
            throw new NotImplementedException();
        }
    }
}
