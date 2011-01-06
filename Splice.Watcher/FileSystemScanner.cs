using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Splice.Data;
using Splice.Reporting;
using TvdbLib;
using TvdbLib.Data;
using System.Text.RegularExpressions;
using Splice.Configuration;

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
                    show.TvdbId = series.Id;

                    show = DataAccess.SaveTVShow(show);
                }

                // Show already in DB
                UpdateShow(show);
            }
        }

        private void UpdateShow(TVShow Show)
        {
            Reporting.Log("Updating show: " + Show.Title);
            DirectoryInfo showDir = new DirectoryInfo(Show.Location);
            DirectoryInfo[] Dirs = showDir.GetDirectories();

            // Pull out the first number in the folder as the season number. Default to 0 (Specials).
            Regex SeasonRegex = new Regex(@"(\d+)", RegexOptions.IgnoreCase);
            foreach (DirectoryInfo Dir in Dirs)
            {
                int SeasonNumber = 0;
                MatchCollection Matches = SeasonRegex.Matches(Dir.Name);
                if (Matches.Count > 0)
                {
                    SeasonNumber = Convert.ToInt32(Matches[0].Captures[0].Value);
                }

                TVSeason Season = UpdateSeason(Dir, Show, SeasonNumber);

                foreach (FileInfo File in Dir.GetFiles())
                {
                    if (ConfigurationManager.CurrentConfiguration.VideoExtensions.Contains(File.Extension))
                    {
                        UpdateFile(File, Show, Season);
                    }
                }
            }
        }

        private TVSeason UpdateSeason(DirectoryInfo Dir, TVShow Show, int SeasonNumber)
        {
            TVSeason Season = DataAccess.GetTVSeason(Show, SeasonNumber);

            if (Season != null)
            {
                return Season;
            }
            else
            {
                Season = new TVSeason()
                {
                    SeasonNumber = SeasonNumber,
                    Title = "",
                    ShowId = Show.Id,
                    Art = ""
                };
                return DataAccess.SaveSeason(Season);
            }
        }

        private void UpdateFile(FileInfo File, TVShow Show, TVSeason Season)
        {
            Regex EpisodeRegex = new Regex(@"^(\d+)");

            MatchCollection Matches = EpisodeRegex.Matches(File.Name);
            int EpisodeNumber;
            if (Matches.Count > 0)
            {
                EpisodeNumber = Convert.ToInt32(Matches[0].Captures[0].Value);

            }
            else
            {
                return;
            }
            TVEpisode Episode = DataAccess.GetTVEpisode(Show, Season, EpisodeNumber);

            if (Episode != null)
            {
                if (Episode.VideoFile.Path == File.FullName)
                {
                    // Compare hashes
                }
                else
                {
                    // Update Path
                }
            }
            else
            {
                // New File
                TvdbEpisode TVDBEpisode = LookupEpisode(Show.TvdbId, Season.SeasonNumber, EpisodeNumber);



                Episode = new TVEpisode()
                {
                    SeasonId = Season.Id,
                    EpisodeNumber = EpisodeNumber,
                    Title = TVDBEpisode.EpisodeName
                };

                // Save Episode
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

        private TvdbEpisode LookupEpisode(int TvdbId, int SeasonNumber, int EpisodeNumber)
        {
            TvdbHandler handler = new TvdbHandler("572AD6335A69FAB2");
            return handler.GetEpisode(TvdbId, SeasonNumber, EpisodeNumber, TvdbEpisode.EpisodeOrdering.DefaultOrder, TvdbLanguage.DefaultLanguage);
        }
    }
}
