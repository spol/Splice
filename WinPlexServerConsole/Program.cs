using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Text;
using WinPlexServer;

namespace WinPlexServerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            const string ServerTestUrl = "http://localhost:32400/";
            PlexServer server = new PlexServer(ServerTestUrl);
//            Console.WriteLine("Listener accepting requests: {0}", listener.RunState == WebServer.State.Started);

            Console.WriteLine("Done. Press any key to exit.");
            Console.ReadLine();
            
        }
    }
}
