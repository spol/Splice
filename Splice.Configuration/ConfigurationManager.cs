using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Splice.Configuration
{
    public static class ConfigurationManager
    {
        public static SpliceConfiguration CurrentConfiguration { get; set; }

        public static string AppConfigPath { get; set; }

        public static string SettingsFilePath { get { return AppConfigPath + "Settings.xml"; } }

        public static string DBFilePath { get { return AppConfigPath + "Data.db"; } }

        public static void LoadConfig()
        {
            string AppData = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            AppConfigPath = AppData + @"\Splice\";

            if (!Directory.Exists(AppConfigPath))
            {
                Directory.CreateDirectory(AppConfigPath);
            }

            if (!File.Exists(SettingsFilePath))
            {
                CurrentConfiguration = SpliceConfiguration.DefaultConfiguration;
                SpliceConfiguration.Serialize(SettingsFilePath, CurrentConfiguration);
            }
            else
            {
                CurrentConfiguration = SpliceConfiguration.Deserialize(SettingsFilePath);
            }
        }
    }
}
