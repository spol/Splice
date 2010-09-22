using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace WinPlexServer
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
                    SQLiteConnectionStringBuilder csb = new SQLiteConnectionStringBuilder();
                    csb.DataSource = "data.db";

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
            SQLiteDataReader reader = ExecuteReader(@"SELECT * FROM collections");
            List<VideoCollection> collections = new List<VideoCollection>();

            while (reader.Read())
            {
                string collectionName = reader.GetString(reader.GetOrdinal("name"));
                int id = reader.GetInt32(reader.GetOrdinal("id"));
                int type = reader.GetInt32(reader.GetOrdinal("type"));
                VideoCollection collection = new VideoCollection(id, collectionName, type);
                collections.Add(collection);
            }

            return collections;
        }

        public static VideoCollection GetVideoCollection(int id)
        {
            using (SQLiteCommand cmd = Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT * FROM collections WHERE id = " + id.ToString();
                SQLiteDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string collectionName = reader.GetString(reader.GetOrdinal("name"));
                    int type = reader.GetInt32(reader.GetOrdinal("type"));
                    return new VideoCollection(id, collectionName, type);
                }
            }

            return null;
        }

        public static List<TVShow> GetTVShows(int collectionId, Filter filter)
        {
            List<TVShow> shows = new List<TVShow>();

            using (SQLiteCommand cmd = Connection.CreateCommand())
            {
                cmd.CommandText = String.Format(filter.Query, collectionId);
                SQLiteDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    TVShow show = new TVShow();
                    show.Id = reader.GetInt32(reader.GetOrdinal("id"));
                    show.Title = reader.GetString(reader.GetOrdinal("title"));
                    shows.Add(show);
                }
            }

            return shows;
        }

        internal static string GetType(int id)
        {
            return (string)GetScalar("SELECT type FROM global_ids WHERE id = " + id.ToString());
        }

        internal static TVShow GetTVShow(int id)
        {
            SQLiteDataReader reader = ExecuteReader("SELECT * FROM tv_shows WHERE id = " + id.ToString());
            TVShow show = new TVShow();
            reader.Read();
            show.Id = id;
            show.Title = reader.GetString(reader.GetOrdinal("title"));
            show.Collection = reader.GetInt32(reader.GetOrdinal("collection"));

            return show;
        }

        internal static List<TVSeason> GetTVSeasons(TVShow show)
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

        internal static List<TVEpisode> GetTVEpisodes(TVSeason season)
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

        internal static VideoFile GetVideoFileFromParent(int parentId)
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

        internal static VideoFile GetVideoFile(int fileId)
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

        internal static TVSeason GetTVSeason(int seasonId)
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
    }
}
