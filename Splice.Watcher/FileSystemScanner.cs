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
        private string TvdbApiKey = "572AD6335A69FAB2";

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
                try
                {
                    ScanCollection(collection);
                }
                catch (IOException Ex)
                {
                    Reporting.Log(Ex.Message);
                }
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

            if (!dirInfo.Exists)
            {
                throw new IOException(String.Format("Collection root doesn't exist. ({0})", collection.Root));
            }

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

            string[] VideoPaths = Directory.GetFiles(Show.Location, "*.*", SearchOption.AllDirectories);

            List<VideoFileInfo> Videos = new List<VideoFileInfo>();
//            Reporting.Log(VideoPaths.Length.ToString());
            foreach (string VidPath in VideoPaths)
            {
                if (!ConfigurationManager.CurrentConfiguration.VideoExtensions.Contains(new FileInfo(VidPath).Extension.Trim('.')))
                {
                    //Reporting.Log("Skipping: " + VidPath);
                    //Reporting.Log(new FileInfo(VidPath).Extension.Trim('.'));
                    continue;
                }
                
                Reporting.Log(VidPath);
                VideoFileInfo VidFile = new VideoFileInfo(VidPath);

                VideoFileInfo CurrentFile = DataAccess.GetVideoFileFromHash(VidFile.Hash);
                Reporting.Log(VidFile.Hash);

                if (CurrentFile != null)
                {
                    // File has been seen before.
                    Reporting.Log("Already seen");

                    if (CurrentFile.Path != VidFile.Path)
                    {
                        Reporting.Log("Updating location.");
                        // Update video file location.
                        CurrentFile.Path = VidFile.Path;
                        DataAccess.SaveVideoFile(CurrentFile);
                    }
                }
                else
                {
                    // new file.
                    string RelativePath = VidPath.Replace(Show.Location, "");

                    int SeasonNumber = -1;
                    int EpisodeNumber = -1;
                    foreach (string Regex in ConfigurationManager.CurrentConfiguration.TVRegexes)
                    {
//                        Reporting.Log(Regex);
                        Regex R = new Regex(Regex);
                        Match M = R.Match(RelativePath);
                        if (M.Success)
                        {
                            if (M.Groups.Count < 1)
                            {
                                continue;
                            }
                            else if (M.Groups.Count == 2)
                            {
                                EpisodeNumber = Convert.ToInt32(M.Groups[1].Value);
                            }
                            else
                            {
                                SeasonNumber = Convert.ToInt32(M.Groups[1].Value);
                                EpisodeNumber = Convert.ToInt32(M.Groups[2].Value);
                            }
                            break;
                        }
                    }

                    if (EpisodeNumber == 0)
                    {
                        Reporting.Log(RelativePath + " does not match any TV regexes in config file.");
                        return;
                    }

                    // check for presence of season
                    TVSeason Season = DataAccess.GetTVSeason(Show, SeasonNumber);

                    if (Season == null)
                    {
                        // insert season if not present.
                        Season = new TVSeason() { SeasonNumber = SeasonNumber, ShowId = Show.Id };
                        Season = DataAccess.SaveSeason(Season);
                        //ODO: Fetch and save artwork for season.
                    }

                    // check for presence of episode
                    TVEpisode Episode = DataAccess.GetTVEpisode(Show, Season, EpisodeNumber);

                    // insert ep if not present
                    if (Episode == null)
                    {
                        TvdbEpisode TvdbEp = LookupEpisode(Show, Season, EpisodeNumber);

                        if (TvdbEp == null)
                        {
                            Reporting.Log(String.Format("Episode not found: {0} - {1}x{2} ({3})", Show.Title, Season.SeasonNumber, EpisodeNumber, VidFile.Path));
                            continue;
                        }
                        Episode = new TVEpisode() { 
                            EpisodeNumber = EpisodeNumber,
                            SeasonId = Season.Id,
                            AirDate = TvdbEp.FirstAired,
                            Rating = TvdbEp.Rating,
                            Summary = TvdbEp.Overview,
                            Title = TvdbEp.EpisodeName,
                            TvdbId = TvdbEp.Id,
                        };
                        Episode = DataAccess.SaveEpisode(Episode);

                        // TODO: Save artwork for episode.
                    }

                    // save video file
                    VidFile = DataAccess.SaveVideoFile(VidFile);

                    // add video file to episode.
                    DataAccess.AssocVideoWithEpisode(VidFile, Episode);
                }

            }

            // TODO: Clean up missing files.

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

        private TvdbSeries LookupShow(string name)
        {
            Reporting.Log("Looking up show: " + name);
            TvdbHandler handler = new TvdbHandler(TvdbApiKey);

            List<TvdbSearchResult> results = handler.SearchSeries(name);

            TvdbSearchResult first = results.First();
            return handler.GetBasicSeries(first.Id, TvdbLanguage.DefaultLanguage, true);
        }

        private TvdbEpisode LookupEpisode(TVShow Show, TVSeason Season, Int32 EpisodeNumber)
        {
            TvdbHandler Handler = new TvdbHandler(TvdbApiKey);

            return Handler.GetEpisode(Show.TvdbId, Season.SeasonNumber, EpisodeNumber, TvdbEpisode.EpisodeOrdering.DefaultOrder, TvdbLanguage.DefaultLanguage);
        }

        private TvdbEpisode LookupEpisode(int TvdbId, int SeasonNumber, int EpisodeNumber)
        {
            TvdbHandler handler = new TvdbHandler("572AD6335A69FAB2");
            return handler.GetEpisode(TvdbId, SeasonNumber, EpisodeNumber, TvdbEpisode.EpisodeOrdering.DefaultOrder, TvdbLanguage.DefaultLanguage);
        }
    }
}
