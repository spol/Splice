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
    public class Config
    {
        int _Version;
        string _StringItem;
        int _IntItem;
        List<string> _Extensions;

        public Config()
        {
            _Extensions = new List<string>();
            _Extensions.Add("mkv");
            _Extensions.Add("avi");
            _Extensions.Add("m4v");
        }
        public static void Serialize(string file, Config c)
        {
            System.Xml.Serialization.XmlSerializer xs
               = new System.Xml.Serialization.XmlSerializer(c.GetType());
            StreamWriter writer = File.CreateText(file);
            xs.Serialize(writer, c);
            writer.Flush();
            writer.Close();
        }
        public static Config Deserialize(string file)
        {
            System.Xml.Serialization.XmlSerializer xs
               = new System.Xml.Serialization.XmlSerializer(
                  typeof(Config));
            StreamReader reader = File.OpenText(file);
            Config c = (Config)xs.Deserialize(reader);
            reader.Close();
            return c;
        }


        [XmlArrayItem("Extension")]
        public List<string> Extensions
        {
            get { return _Extensions; }
            set { _Extensions = value; }
        }
    }
}
