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

        public static List<TVSeason> GetTVSeasons(TVShow show)
        {
            SQLiteDataReader reader = ExecuteReader("SELECT * FROM tv_seasons WHERE showId = " + show.Id.ToString());

            List<TVSeason> seasons = new List<TVSeason>();
            while (reader.Read())
            {
                TVSeason season = new TVSeason();
                season.Id = reader.GetInt32(reader.GetOrdinal("id"));
                season.Title = reader.GetString(reader.GetOrdinal("title"));
                season.SeasonNumber = reader.GetInt32(reader.GetOrdinal("seasonNumber"));

                seasons.Add(season);
            }

            return seasons;
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
                    Duration = Convert.ToInt32(Row["duration"]),
                    EpisodeNumber = EpisodeNumber,
                    Id = Convert.ToInt32(Row["id"]),
                    Rating = Convert.ToSingle(Row["rating"]),
                    SeasonId = Season.Id,
                    Summary = Row["Summary"].ToString(),
                    Title = Row["title"].ToString(),
                    VideoFile = new VideoFile()
                    {
                        Path = Row["path"].ToString()
                    }
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

                episode.VideoFile = DataAccess.GetVideoFileFromParent(episode.Id);

                episodes.Add(episode);
            }

            return episodes;
        }

        public static VideoFile GetVideoFileFromParent(int parentId)
        {
            SQLiteDataReader reader = ExecuteReader("SELECT * FROM video_files WHERE parentId = " + parentId.ToString());

            VideoFile vid = new VideoFile();

            reader.Read();

            vid.AspectRatio = reader.GetFloat(reader.GetOrdinal("aspectRatio"));
            vid.AudioChannels = reader.GetFloat(reader.GetOrdinal("audioChannels"));
            vid.AudioCodec = reader.GetString(reader.GetOrdinal("audioCodec"));
            vid.Bitrate = reader.GetInt32(reader.GetOrdinal("bitrate"));
            vid.Duration = reader.GetInt32(reader.GetOrdinal("duration"));
            vid.Id = reader.GetInt32(reader.GetOrdinal("id"));
            vid.VideoCodec = reader.GetString(reader.GetOrdinal("videoCodec"));
            vid.VideoFrameRate = reader.GetString(reader.GetOrdinal("videoFrameRate"));
            vid.VideoResolution = reader.GetString(reader.GetOrdinal("videoResolution"));
            vid.Path = reader.GetString(reader.GetOrdinal("path"));
            vid.Size = reader.GetInt32(reader.GetOrdinal("size"));

            return vid;
        }

        public static VideoFile GetVideoFile(int fileId)
        {
            SQLiteDataReader reader = ExecuteReader("SELECT * FROM video_files WHERE id = " + fileId.ToString());

            VideoFile vid = new VideoFile();

            reader.Read();

            vid.AspectRatio = reader.GetFloat(reader.GetOrdinal("aspectRatio"));
            vid.AudioChannels = reader.GetFloat(reader.GetOrdinal("audioChannels"));
            vid.AudioCodec = reader.GetString(reader.GetOrdinal("audioCodec"));
            vid.Bitrate = reader.GetInt32(reader.GetOrdinal("bitrate"));
            vid.Duration = reader.GetInt32(reader.GetOrdinal("duration"));
            vid.Id = reader.GetInt32(reader.GetOrdinal("id"));
            vid.VideoCodec = reader.GetString(reader.GetOrdinal("videoCodec"));
            vid.VideoFrameRate = reader.GetString(reader.GetOrdinal("videoFrameRate"));
            vid.VideoResolution = reader.GetString(reader.GetOrdinal("videoResolution"));
            vid.Path = reader.GetString(reader.GetOrdinal("path"));
            vid.Size = reader.GetInt32(reader.GetOrdinal("size"));

            return vid;
        }

        public static TVSeason GetTVSeason(int seasonId)
        {
            SQLiteDataReader reader = ExecuteReader("SELECT * FROM tv_seasons WHERE id = " + seasonId.ToString());
            TVSeason season = new TVSeason();
            reader.Read();
            season.Id = seasonId;
            season.Title = reader.GetString(reader.GetOrdinal("title"));
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
                Season.Title = Convert.ToString(SeasonRow["title"] == DBNull.Value ? "" : SeasonRow["title"]);
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
            SQLiteCommand cmd = Connection.CreateCommand();
            cmd.CommandText = String.Format(@"INSERT INTO tv_shows (title, tvdbId, collection, studio, contentRating, summary, rating, year, thumb, art, banner, 
duration, originallyAvailableAt, lastUpdated, location) VALUES ( 
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
                @Location); SELECT last_insert_rowid() AS ShowId;");
            cmd.Parameters.Add("@Title", DbType.AnsiString);
            cmd.Parameters["@Title"].Value = show.Title;
            // TODO
            cmd.Parameters.Add("@TvdbId", DbType.Int32);
            cmd.Parameters["@TvdbId"].Value = 0;
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

            show.Id = Convert.ToInt32(cmd.ExecuteScalar());
            return show;
        }

        public static TVSeason SaveSeason(TVSeason Season)
        {
            SQLiteCommand cmd = Connection.CreateCommand();
            cmd.CommandText = String.Format(@"INSERT INTO tv_seasons (seasonNumber, title, showId, art) VALUES ( 
                @SeasonNumber,
                @Title,
                @ShowId,
                @Art); SELECT last_insert_rowid() AS ShowId;");
            cmd.Parameters.Add(new SQLiteParameter("@SeasonNumber", DbType.Int32) { Value = Season.SeasonNumber });
            cmd.Parameters.Add(new SQLiteParameter("@Title", DbType.String) { Value = Season.Title });
            cmd.Parameters.Add(new SQLiteParameter("@ShowId", DbType.Int32) { Value = Season.ShowId });
            cmd.Parameters.Add(new SQLiteParameter("@Art", DbType.String) { Value = Season.Art });

            Season.Id = Convert.ToInt32(cmd.ExecuteScalar());
            return Season;
        }
    }
}
