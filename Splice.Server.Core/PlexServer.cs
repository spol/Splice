using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using ZeroconfService;

namespace Splice.Server
{
    public class PlexServer
    {
        private WebServer listener = null;

        public PlexServer(string ServerTestUrl)
        {
            Router router = new Router();
            router.AddController("library", new Library());
            router.AddController("resources", new Resources());
            listener = new WebServer(new Uri(ServerTestUrl), router);
            listener.Start();

            String domain = "";
            String type = "_plexmediasvr._tcp";
            String name = "WinPlex Media Server";
            int port = 32400;

            NetService publishService = new NetService(domain, type, name, port);

            /* HARDCODE TXT RECORD */
            System.Collections.Hashtable dict = new System.Collections.Hashtable();
            dict.Add("txtvers", "1");
            publishService.TXTRecordData = NetService.DataFromTXTRecordDictionary(dict);

            publishService.Publish();

        }
    }
}
