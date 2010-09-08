using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace WinPlexServer
{
    public static class DataAccess
    {
        public static List<VideoCollection> GetVideoCollections()
        {
            SQLiteConnection conn = new SQLiteConnection("Data Source=data.db");
            conn.Open();
            List<VideoCollection> collections = new List<VideoCollection>();

            using (SQLiteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"SELECT * FROM collections";
                SQLiteDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string collectionName = reader.GetString(reader.GetOrdinal("name"));
                    int id = reader.GetInt32(reader.GetOrdinal("id"));
                    int type = reader.GetInt32(reader.GetOrdinal("type"));
                    VideoCollection collection = new VideoCollection(id, collectionName, type);
                    collections.Add(collection);
                }
            }

            return collections;
        }

        public static VideoCollection GetVideoCollection(int id)
        {
            SQLiteConnection conn = new SQLiteConnection("Data Source=data.db");
            conn.Open();

            using (SQLiteCommand cmd = conn.CreateCommand())
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
    }
}
