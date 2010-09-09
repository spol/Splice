﻿using System;
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
                season.Title = reader.GetString(reader.GetOrdinal("seasonName"));
                season.SeasonNumber = reader.GetInt32(reader.GetOrdinal("seasonNumber"));

                seasons.Add(season);
            }

            return seasons;
        }

        public static List<TVShow> GetTVShows(int collectionId, Filter filter)
        {
            SQLiteConnection conn = new SQLiteConnection("Data Source=data.db");
            conn.Open();
            List<TVShow> shows = new List<TVShow>();

            using (SQLiteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = filter.Query;
                SQLiteDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string showName = reader.GetString(reader.GetOrdinal("showName"));
                    int id = reader.GetInt32(reader.GetOrdinal("id"));
                    int type = reader.GetInt32(reader.GetOrdinal("type"));
                    TVShow show = new TVShow(id, showName);
                    shows.Add(show);
                }
            }

            return shows;
        }
    }
}
