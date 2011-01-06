using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Splice.Configuration
{
    [Serializable]
    public class SpliceConfiguration
    {
        List<string> _Extensions;

        public static SpliceConfiguration DefaultConfiguration
        {
            get
            {
                SpliceConfiguration Config = new SpliceConfiguration();
                Config.VideoExtensions.Add("mkv");
                Config.VideoExtensions.Add("avi");
                Config.VideoExtensions.Add("m4v");

                return Config;
           }
        }

        public SpliceConfiguration()
        {
            _Extensions = new List<string>();
        }

        public static void Serialize(string file, SpliceConfiguration c)
        {
            System.Xml.Serialization.XmlSerializer xs
               = new System.Xml.Serialization.XmlSerializer(c.GetType());
            StreamWriter writer = File.CreateText(file);
            xs.Serialize(writer, c);
            writer.Flush();
            writer.Close();
        }
        public static SpliceConfiguration Deserialize(string file)
        {
            System.Xml.Serialization.XmlSerializer xs
               = new System.Xml.Serialization.XmlSerializer(
                  typeof(SpliceConfiguration));
            StreamReader reader = File.OpenText(file);
            SpliceConfiguration c = (SpliceConfiguration)xs.Deserialize(reader);
            reader.Close();
            return c;
        }


        [XmlArrayItem("Extension")]
        public List<string> VideoExtensions
        {
            get { return _Extensions; }
            set { _Extensions = value; }
        }
    }
}
