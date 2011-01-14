using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Data;
using Splice.Data.Filters;
using Splice.Configuration;

namespace Splice.Data
{
    public static class DataAccess
    {
        private static SQLiteConnection _connection;
        private static SQLiteConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    if (!File.Exists(ConfigurationManager.DBFilePath))
                    {
                        File.Copy("data.db", ConfigurationManager.DBFilePath);
                    }
                    SQLiteConnectionStringBuilder csb = new SQLiteConnectionStringBuilder();
                    csb.DataSource = ConfigurationManager.DBFilePath;

                    _connection = new SQLiteConnection(csb.ToString());
                    _connection.Open();
                }
                return _connection;
            }
        }

        private static SQLiteDataReader ExecuteReader(string query)
        {
            SQLiteCommand cmd = Connection.CreateCommand();
            
            cmd.CommandText = query;
            return cmd.ExecuteReader();
        }

        private static object GetScalar(string query)
        {
            SQLiteDataReader reader = ExecuteReader(query);

            reader.Read();

            Type type = reader.GetFieldType(0);

            switch (type.Name)
            {
                case "String":
                    return reader.GetString(0);
            }
            return null;
        }

        private static Int32 GetNewGlobalId(string Type)
        {
            SQLiteCommand cmd = Connection.CreateCommand();
            cmd.CommandText = String.Format(@"INSERT INTO global_ids (type) VALUES (@Type); SELECT last_insert_rowid() AS Id;");
            cmd.Parameters.Add(new SQLiteParameter("@Type", DbType.String ) { Value = Type });

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public static List<VideoCollection> GetVideoCollections()
        {
            List<VideoCollection> collections = new List<VideoCollection>();

            using (SQLiteCommand cmd = Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT * FROM collections";

                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);

                DataTable collectionsTable = new DataTable();
                da.Fill(collectionsTable);
                
                foreach (DataRow collectionRow in collectionsTable.Rows)
                {
                    VideoCollection collection = new VideoCollection();

                    collection.Id = Convert.ToInt32(collectionRow["id"]);
                    collection.Type = collectionRow["type"].ToString();
                    collection.Title = collectionRow["title"].ToString();
                    collection.Root = collectionRow["root"].ToString();

                    collections.Add(collection);
                }
            }
            return collections;
        }

        public static VideoCollection GetVideoCollection(int id)
        {
            using (SQLiteCommand cmd = Connection.CreateCommand())
            {
                cmd.CommandText = String.Format(@"SELECT * FROM collections WHERE id = {0}", id);

                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);

                DataTable collectionsTable = new DataTable();
                da.Fill(collectionsTable);

                if (collectionsTable.Rows.Count == 0)
                {
                    return null;
                }
                else
                {
                    DataRow collectionRow = collectionsTable.Rows[0];
                    VideoCollection collection = new VideoCollection();
                    collection.Id = Convert.ToInt32(collectionRow["id"]);
                    collection.Type = collectionRow["type"].ToString();
                    collection.Title = collectionRow["title"].ToString();
                    collection.Root = collectionRow["root"].ToString();
                    return collection;
                }
            }
        }

        public static List<TVShow> GetTVShows(int collectionId, Filter filter)
        {
            List<TVShow> shows = new List<TVShow>();

            using (SQLiteCommand cmd = Connection.CreateCommand())
            {
                cmd.CommandText = String.Format(filter.Query, collectionId);

                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);

                DataTable showsTable = new DataTable();
                da.Fill(showsTable);

                foreach (DataRow showRow in showsTable.Rows)
                {
                    TVShow show = new TVShow();
                    
                    show.Id = Convert.ToInt32(showRow["id"]);
                    show.Title = Convert.ToString(showRow["title"]);
                    show.Banner = Convert.ToString(showRow["banner"] == DBNull.Value ? "" : showRow["banner"]);
                    show.Art = Convert.ToString(showRow["art"] == DBNull.Value ? "" : showRow["art"]);
                    show.Thumb = Convert.ToString(showRow["thumb"] == DBNull.Value ? "" : showRow["thumb"]);
                    show.LastUpdated = Convert.ToInt32(showRow["lastUpdated"]);
                    show.Collection = Convert.ToInt32(showRow["collection"]);
                    show.ContentRating = showRow["contentRating"].ToString();
                    show.Duration = Convert.ToInt32(showRow["duration"]);
                    show.LastUpdated = Convert.ToInt32(showRow["lastUpdated"]);
                    // TODO.
                    show.LeafCount = 0;
                    show.OriginallyAvailableAt = Convert.ToDateTime(showRow["originallyAvailableAt"]);
                    show.Rating = Convert.ToSingle(showRow["rating"]);
                    show.Studio = showRow["studio"].ToString();
                    show.Summary = showRow["summary"].ToString();
                    // TODO
                    show.ViewedLeafCount = 0;
                    shows.Add(show);
                }
            }

            return shows;
        }

        public static string GetType(int id)
        {
            return (string)GetScalar("SELECT type FROM global_ids WHERE id = " + id.ToString());
        }

        public static TVShow GetTVShow(int id)
        {
            SQLiteCommand cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM tv_shows WHERE id = " + id.ToString();

            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);

            DataTable showsTable = new DataTable();
            da.Fill(showsTable);

            if (showsTable.Rows.Count != 1)
            {
                return null;
            }
            else
            {
                DataRow showRow = showsTable.Rows[0];
                TVShow show = new TVShow();
                show.Id = id;
                show.Collection = Convert.ToInt32(showRow["collection"]);

                show.Title = Convert.ToString(showRow["title"]);
                show.Banner = Convert.ToString(showRow["banner"] == DBNull.Value ? "" : showRow["banner"]);
                show.Art = Convert.ToString(showRow["art"] == DBNull.Value ? "" : showRow["art"]);
                show.Thumb = Convert.ToString(showRow["thumb"] == DBNull.Value ? "" : showRow["thumb"]);
                show.LastUpdated = Convert.ToInt32(showRow["lastUpdated"]);

                return show;
            }
        }

        public static TVShow GetTVShowFromPath(string Path)
        {
            SQLiteCommand cmd = Connection.CreateCommand();
            cmd.CommandText = String.Format("SELECT * FROM tv_shows WHERE lower(location) = lower('{0}')", Path);

            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);

            DataTable showsTable = new DataTable();
            da.Fill(showsTable);

            if (showsTable.Rows.Count != 1)
            {
                return null;
            }
            else
            {
                DataRow showRow = showsTable.Rows[0];
                TVShow show = new TVShow();
                show.Id = Convert.ToInt32(showRow["id"]);
                show.Collection = Convert.ToInt32(showRow["collection"]);

                show.Title = Convert.ToString(showRow["title"]);
                show.Banner = Convert.ToString(showRow["banner"] == DBNull.Value ? "" : showRow["banner"]);
                show.Art = Convert.ToString(showRow["art"] == DBNull.Value ? "" : showRow["art"]);
                show.Thumb = Convert.ToString(showRow["thumb"] == DBNull.Value ? "" : showRow["thumb"]);
                show.LastUpdated = Convert.ToInt32(showRow["lastUpdated"]);
                show.Location = showRow["location"].ToString();
                show.TvdbId = Convert.ToInt32(showRow["tvdbId"]);

                return show;
            }
        }

        public static List<TVSeason> GetTVSeasons(TVShow Show)
        {
            SQLiteCommand cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM tv_seasons WHERE showId = @Id ORDER BY seasonNumber > 0 DESC, seasonNumber ASC";

            cmd.Parameters.Add(new SQLiteParameter("@Id", DbType.Int32) { Value = Show.Id });

            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);

            DataTable SeasonsTable = new DataTable();
            da.Fill(SeasonsTable);

            List<TVSeason> Seasons = new List<TVSeason>();
            foreach (DataRow Row in SeasonsTable.Rows)
            {
                Seasons.Add(new TVSeason(Row));
            }
            return Seasons;

        }

        public static TVEpisode GetTVEpisode(TVShow Show, TVSeason Season, int EpisodeNumber)
        {
            SQLiteCommand cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM tv_episodes WHERE episodeNumber = @EpisodeNumber AND seasonId = @SeasonId";

            cmd.Parameters.Add(new SQLiteParameter("@EpisodeNumber", DbType.Int32) { Value = EpisodeNumber });
            cmd.Parameters.Add(new SQLiteParameter("@SeasonId", DbType.Int32) { Value = Season.Id });

            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);

            DataTable EpisodeTable = new DataTable();
            da.Fill(EpisodeTable);

            if (EpisodeTable.Rows.Count == 1)
            {
                DataRow Row = EpisodeTable.Rows[0];
                TVEpisode Episode = new TVEpisode()
                {
                    AirDate = Convert.ToDateTime(Row["airDate"]),
                    //Duration = Convert.ToInt32(Row["duration"]),
                    EpisodeNumber = EpisodeNumber,
                    Id = Convert.ToInt32(Row["id"]),
                    Rating = Convert.ToSingle(Row["rating"]),
                    SeasonId = Season.Id,
                    Summary = Row["Summary"].ToString(),
                    Title = Row["title"].ToString(),
                    //VideoFile = new VideoFile()
                    //{
                    //    Path = Row["path"].ToString()
                    //}
                };

                return Episode;
            }

            return null;
        }

        public static List<TVEpisode> GetTVEpisodes(TVSeason season)
        {
            SQLiteDataReader reader = ExecuteReader("SELECT * FROM tv_episodes WHERE seasonId = " + season.Id.ToString());

            List<TVEpisode> episodes = new List<TVEpisode>();
            while (reader.Read())
            {
                TVEpisode episode = new TVEpisode();
                episode.Id = reader.GetInt32(reader.GetOrdinal("id"));
                episode.Title = reader.GetString(reader.GetOrdinal("title"));
                episode.EpisodeNumber = reader.GetInt32(reader.GetOrdinal("episodeNumber"));
                episode.SeasonId = reader.GetInt32(reader.GetOrdinal("seasonId"));
                episode.Rating = reader.GetFloat(reader.GetOrdinal("rating"));
                episode.Summary = reader.GetString(reader.GetOrdinal("summary"));
                //episode.EpisodeNumber = reader.GetInt32(reader.GetOrdinal("episodeNumber"));
                //episode.EpisodeNumber = reader.GetInt32(reader.GetOrdinal("episodeNumber"));
                //episode.EpisodeNumber = reader.GetInt32(reader.GetOrdinal("episodeNumber"));

                episode.VideoFiles = DataAccess.GetVideoFilesForEpisode(episode.Id);

                episodes.Add(episode);
            }

            return episodes;
        }

        public static List<VideoFileInfo> GetVideoFilesForEpisode(int EpisodeNumber)
        {
            SQLiteCommand cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM video_files v LEFT JOIN episode2video e2v ON v.id = e2v.videoId WHERE e2v.episodeId = @EpisodeNumber";

            cmd.Parameters.Add(new SQLiteParameter("@EpisodeNumber", DbType.Int32) { Value = EpisodeNumber });

            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);

            DataTable VideoFileTable = new DataTable();
            da.Fill(VideoFileTable);

            List<VideoFileInfo> Files = new List<VideoFileInfo>();

            foreach (DataRow Row in VideoFileTable.Rows)
            {
                Files.Add(new VideoFileInfo(Row["path"].ToString()));
            }

            return Files;
        }

        public static VideoFileInfo GetVideoFile(int FileId)
        {
            SQLiteCommand cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM video_files WHERE id = @Id";

            cmd.Parameters.Add(new SQLiteParameter("@Id", DbType.Int32) { Value = FileId });

            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);

            DataTable VideoFileTable = new DataTable();
            da.Fill(VideoFileTable);

            if (VideoFileTable.Rows.Count == 1)
            {
                DataRow Row = VideoFileTable.Rows[0];

                VideoFileInfo VideoFile = new VideoFileInfo(Row);

                return VideoFile;
            }
            else {
                return null;
            }
        }

        public static TVSeason GetTVSeason(int seasonId)
        {
            SQLiteDataReader reader = ExecuteReader("SELECT * FROM tv_seasons WHERE id = " + seasonId.ToString());
            TVSeason season = new TVSeason();
            reader.Read();
            season.Id = seasonId;
            season.SeasonNumber = reader.GetInt32(reader.GetOrdinal("seasonNumber"));
            season.ShowId = reader.GetInt32(reader.GetOrdinal("showId"));

            return season;
        }

        public static TVSeason GetTVSeason(TVShow Show, int SeasonNumber)
        {
            SQLiteCommand cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM tv_seasons WHERE seasonNumber = @SeasonNumber AND showId = @ShowId";

            cmd.Parameters.Add(new SQLiteParameter("@SeasonNumber", DbType.Int32) { Value = SeasonNumber });
            cmd.Parameters.Add(new SQLiteParameter("@ShowId", DbType.Int32) { Value = Show.Id });

            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);

            DataTable SeasonsTable = new DataTable();
            da.Fill(SeasonsTable);

            if (SeasonsTable.Rows.Count == 1)
            {
                DataRow SeasonRow = SeasonsTable.Rows[0];
                TVSeason Season = new TVSeason();
                Season.Id = Convert.ToInt32(SeasonRow["id"]);
                Season.SeasonNumber = Convert.ToInt32(SeasonRow["seasonNumber"]);
                Season.ShowId = Convert.ToInt32(SeasonRow["showId"]);
                //Season.Art = Convert.ToString(showRow["art"] == DBNull.Value ? "" : showRow["art"]);

                return Season;
            }
            else 
            {
                return null;
            }

        }

        public static TVShow SaveTVShow(TVShow show)
        {
            Int32 NewId = GetNewGlobalId("show");
            
            SQLiteCommand cmd = Connection.CreateCommand();
            cmd.CommandText = String.Format(@"INSERT INTO tv_shows (id, title, tvdbId, collection, studio, contentRating, summary, rating, year, thumb, art, banner, 
duration, originallyAvailableAt, lastUpdated, location) VALUES (
                @Id,
                @Title,
                @TvdbId,
                @Collection,
                @Studio,
                @ContentRating,
                @Summary,
                @Rating,
                @Year,
                @Thumb,
                @Art,
                @Banner,
                @Duration,
                @AirDate,
                @LastUpdated,
                @Location);");
            cmd.Parameters.Add(new SQLiteParameter("@Id", DbType.Int32) { Value = NewId });
            cmd.Parameters.Add("@Title", DbType.AnsiString);
            cmd.Parameters["@Title"].Value = show.Title;
            // TODO
            cmd.Parameters.Add("@TvdbId", DbType.Int32);
            cmd.Parameters["@TvdbId"].Value = show.TvdbId;
            cmd.Parameters.Add("@Collection", DbType.Int32);
            cmd.Parameters["@Collection"].Value = show.Collection;
            cmd.Parameters.Add("@Studio", DbType.String);
            cmd.Parameters["@Studio"].Value = show.Studio;
            cmd.Parameters.Add("@ContentRating", DbType.AnsiString);
            cmd.Parameters["@ContentRating"].Value = show.ContentRating;
            cmd.Parameters.Add("@Summary", DbType.String);
            cmd.Parameters["@Summary"].Value = show.Summary;
            cmd.Parameters.Add("@Rating", DbType.Double);
            cmd.Parameters["@Rating"].Value = show.Rating;
            cmd.Parameters.Add("@Year", DbType.Int32);
            cmd.Parameters["@Year"].Value = show.Year;
            cmd.Parameters.Add("@Thumb", DbType.String);
            cmd.Parameters["@Thumb"].Value = show.Thumb;
            cmd.Parameters.Add("@Art", DbType.String);
            cmd.Parameters["@Art"].Value = show.Art;
            cmd.Parameters.Add("@Banner", DbType.String);
            cmd.Parameters["@Banner"].Value = show.Banner;
            cmd.Parameters.Add("@Duration", DbType.Int32);
            cmd.Parameters["@Duration"].Value = show.Duration;
            cmd.Parameters.Add("@AirDate", DbType.DateTime);
            cmd.Parameters["@AirDate"].Value = show.OriginallyAvailableAt;
            cmd.Parameters.Add("@LastUpdated", DbType.Int32);
            cmd.Parameters["@LastUpdated"].Value = show.LastUpdated;
            cmd.Parameters.Add("@Location", DbType.String);
            cmd.Parameters["@Location"].Value = show.Location;

            cmd.ExecuteNonQuery();
            show.Id = NewId;
            return show;
        }

        public static TVSeason SaveSeason(TVSeason Season)
        {
            Int32 NewId = GetNewGlobalId("season");
            SQLiteCommand cmd = Connection.CreateCommand();
            cmd.CommandText = String.Format(@"INSERT INTO tv_seasons (id, seasonNumber, title, showId, art) VALUES ( 
                @Id,
                @SeasonNumber,
                @Title,
                @ShowId,
                @Art);");
            cmd.Parameters.Add(new SQLiteParameter("@Id", DbType.Int32) { Value = NewId });
            cmd.Parameters.Add(new SQLiteParameter("@SeasonNumber", DbType.Int32) { Value = Season.SeasonNumber });
            cmd.Parameters.Add(new SQLiteParameter("@Title", DbType.String) { Value = Season.Title });
            cmd.Parameters.Add(new SQLiteParameter("@ShowId", DbType.Int32) { Value = Season.ShowId });
            cmd.Parameters.Add(new SQLiteParameter("@Art", DbType.String) { Value = Season.Art });

            cmd.ExecuteNonQuery();
            Season.Id = NewId;
            return Season;
        }

        public static VideoFileInfo GetVideoFileFromHash(string Hash)
        {
            SQLiteCommand cmd = Connection.CreateCommand();
            cmd.CommandText = String.Format(@"SELECT * FROM video_files WHERE fileHash = @Hash;");
            cmd.Parameters.Add(new SQLiteParameter("@Hash", DbType.String) { Value = Hash });

            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);

            DataTable FilesTable = new DataTable();
            da.Fill(FilesTable);

            if (FilesTable.Rows.Count == 1)
            {
                DataRow FileRow = FilesTable.Rows[0];
                VideoFileInfo VideoFile = GetVideoFile(Convert.ToInt32(FileRow["id"]));

                return VideoFile;
            }
            else
            {
                return null;
            }

            
        }

        public static TVEpisode SaveEpisode(TVEpisode Episode)
        {
            Int32 Id = GetNewGlobalId("episode");

            SQLiteCommand cmd = Connection.CreateCommand();
            cmd.CommandText = String.Format(@"INSERT INTO tv_episodes (id, title, episodeNumber, seasonId, summary, rating, airDate) VALUES ( 
                @Id,
                @Title,
                @episodeNumber,
                @SeasonId,
                @Summary,
                @Rating,
                @AirDate);");
            cmd.Parameters.Add(new SQLiteParameter("@Id", DbType.String) { Value = Id });
            cmd.Parameters.Add(new SQLiteParameter("@Title", DbType.String) { Value = Episode.Title });
            cmd.Parameters.Add(new SQLiteParameter("@EpisodeNumber", DbType.Int32) { Value = Episode.EpisodeNumber });
            cmd.Parameters.Add(new SQLiteParameter("@SeasonId", DbType.Int32) { Value = Episode.SeasonId });
            cmd.Parameters.Add(new SQLiteParameter("@Summary", DbType.String) { Value = Episode.Summary });
            cmd.Parameters.Add(new SQLiteParameter("@Rating", DbType.Double) { Value = Episode.Rating });
            cmd.Parameters.Add(new SQLiteParameter("@AirDate", DbType.DateTime) { Value = Episode.AirDate });

            cmd.ExecuteNonQuery();
            Episode.Id = Id;
            return Episode;
        }

        public static VideoFileInfo SaveVideoFile(VideoFileInfo VidFile)
        {
            SQLiteCommand cmd = Connection.CreateCommand();

            if (VidFile.Id == 0)
            {
                VidFile.Id = GetNewGlobalId("videofile");

                cmd.CommandText = String.Format(@"INSERT INTO video_files (id, duration, bitrate, aspectRatio, audioChannels, audioCodec, videoCodec, videoResolution,
videoFrameRate, path, size, fileHash) VALUES (
                @Id,
                @Duration,
                @Bitrate,
                @AspectRatio,
                @AudioChannels,
                @AudioCodec,
                @VideoCodec,
                @VideoResolution,
                @VideoFrameRate,
                @Path,
                @Size,
                @FileHash);");
            }
            else
            {
                cmd.CommandText = String.Format(@"UPDATE video_files SET
                    duration = @Duration,
                    bitrate = @Bitrate,
                    aspectRatio = @AspectRatio,
                    audioChannels = @AudioChannels,
                    audioCodec = @AudioCodec,
                    videoCodec = @VideoCodec,
                    videoResolution = @VideoResolution,
                    videoFrameRate = @VideoFrameRate,
                    path = @Path,
                    size = @Size,
                    fileHash = @FileHash
                    WHERE id = @Id;");
            }
            cmd.Parameters.Add(new SQLiteParameter("@Id", DbType.Int32) { Value = VidFile.Id });
            cmd.Parameters.Add(new SQLiteParameter("@Duration", DbType.Int32) { Value = VidFile.Duration });
            cmd.Parameters.Add(new SQLiteParameter("@Bitrate", DbType.Int32) { Value = VidFile.Bitrate });
            cmd.Parameters.Add(new SQLiteParameter("@AspectRatio", DbType.Double) { Value = VidFile.AspectRatio });
            cmd.Parameters.Add(new SQLiteParameter("@AudioChannels", DbType.Double) { Value = VidFile.AudioChannels });
            cmd.Parameters.Add(new SQLiteParameter("@AudioCodec", DbType.String) { Value = VidFile.AudioCodec });
            cmd.Parameters.Add(new SQLiteParameter("@VideoCodec", DbType.String) { Value = VidFile.VideoCodec });
            cmd.Parameters.Add(new SQLiteParameter("@VideoResolution", DbType.String) { Value = VidFile.VideoResolution });
            cmd.Parameters.Add(new SQLiteParameter("@VideoFrameRate", DbType.String) { Value = VidFile.VideoFrameRate });
            cmd.Parameters.Add(new SQLiteParameter("@Path", DbType.String) { Value = VidFile.Path });
            cmd.Parameters.Add(new SQLiteParameter("@Size", DbType.Int32) { Value = VidFile.Size });
            cmd.Parameters.Add(new SQLiteParameter("@FileHash", DbType.String) { Value = VidFile.Hash });

            cmd.ExecuteNonQuery();
            return VidFile;
        }

        public static Boolean AssocVideoWithEpisode(VideoFileInfo VidFile, TVEpisode Episode)
        {
            SQLiteCommand cmd = Connection.CreateCommand();
            cmd.CommandText = String.Format(@"INSERT INTO episode2video (episodeId, videoId) VALUES ( 
                @EpisodeId,
                @VideoId);");
            cmd.Parameters.Add(new SQLiteParameter("@EpisodeId", DbType.Int32) { Value = Episode.Id });
            cmd.Parameters.Add(new SQLiteParameter("@VideoId", DbType.Int32) { Value = VidFile.Id });

            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
