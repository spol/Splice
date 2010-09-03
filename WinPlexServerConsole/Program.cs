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
            using (WebServer listener = new WebServer(new Uri(ServerTestUrl), new Router()))
            {
                //listener.IncomingRequest += WebServer_IncomingRequest;
                listener.Start();
                Console.WriteLine("Listener accepting requests: {0}", listener.RunState == WebServer.State.Started);
                //Console.WriteLine("Making requests...");
                //while (true)
                //{
                //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ServerTestUrl);
                //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                //    using (Stream responseStream = response.GetResponseStream())
                //    using (StreamReader responseStreamReader = new StreamReader(responseStream, Encoding.UTF8))
                //    {
                //        Console.WriteLine(responseStreamReader.ReadToEnd());
                //    }
                //    System.Threading.Thread.Sleep(1000);
                //}

                Console.WriteLine("Done. Press any key to exit.");
                Console.ReadLine();
            }
        }
    }
}
