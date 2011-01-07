using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Text;
using Splice.Server;
using Splice.Watcher;
using Splice.Configuration;

namespace Splice.Server.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            const string ServerTestUrl = "http://+:32400/";
            PlexServer server = new PlexServer(ServerTestUrl);

            ConsoleLogger logger = new ConsoleLogger();
            ConfigurationManager.LoadConfig();

            FileSystemScanner fsWatcher = new FileSystemScanner(logger);
            
            //fsWatcher.ScanAll();

            System.Console.WriteLine("Done. Press any key to exit.");
            System.Console.ReadLine();

            
        }
    }
}
