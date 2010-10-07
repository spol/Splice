using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Text;
using Splice.Server;
using Splice.Watcher;

namespace Splice.Server.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            const string ServerTestUrl = "http://localhost:32400/";
            PlexServer server = new PlexServer(ServerTestUrl);
//            Console.WriteLine("Listener accepting requests: {0}", listener.RunState == WebServer.State.Started);

            ConsoleLogger logger = new ConsoleLogger();

            FileSystemScanner fsWatcher = new FileSystemScanner(logger);

            fsWatcher.ScanAll();

            System.Console.WriteLine("Done. Press any key to exit.");
            System.Console.ReadLine();

            
        }
    }
}
