using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Splice.Data;
using Splice.Reporting;
using TvdbLib;
using TvdbLib.Data;

namespace Splice.Watcher
{
    public class FileSystemScanner
    {
        private ILogger Reporting;

        public FileSystemScanner(ILogger reporting)
        {
            Reporting = reporting;
        }

        public void ScanAll()
        {
            Reporting.Log("Scanning...");
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
                if (show == null)
                {
                    // New Show.
                    TvdbSeries series = LookupShow(showDir.Name);
                    Reporting.Log("Matched: " + series.SeriesName);
                    show = new TVShow();
                    show.Collection = collection.Id;
                    show.Art = series.FanartPath;
                    show.Banner = series.BannerPath;
                    show.ContentRating = series.ContentRating;
                    show.Duration = Convert.ToInt32(series.Runtime);
                    show.LastUpdated = DateTime.Now.Timestamp();
                    show.LeafCount = 0;
                    show.Location = showDir.FullName;
                    show.OriginallyAvailableAt = series.FirstAired;
                    show.Rating = series.Rating;
                    show.Studio = series.Network;
                    show.Summary = series.Overview;
                    show.Thumb = series.PosterPath;
                    show.Title = series.SeriesName;
                    show.ViewedLeafCount = 0;

                    show = DataAccess.SaveTVShow(show);
                }

                // Show already in DB
                UpdateShow(show);
            }
        }

        private void UpdateShow(TVShow show)
        {
            Reporting.Log("Updating show: " + show.Title);
            DirectoryInfo showDir = new DirectoryInfo(show.Location);
            FileInfo[] files = showDir.GetFiles();

            foreach (FileInfo file in files)
            {
            }
        }

        private TvdbSeries LookupShow(string name)
        {
            Reporting.Log("Looking up show: " + name);
            TvdbHandler handler = new TvdbHandler("572AD6335A69FAB2");

            List<TvdbSearchResult> results = handler.SearchSeries(name);

            TvdbSearchResult first = results.First();
            return handler.GetBasicSeries(first.Id, TvdbLanguage.DefaultLanguage, true);
        }
    }
}
