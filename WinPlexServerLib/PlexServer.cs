using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace WinPlexServer
{
    public class PlexServer
    {
        private WebServer listener = null;

        public PlexServer(string ServerTestUrl)
        {
            Router router = new Router();
            router.AddSection("library", new Library());
            listener = new WebServer(new Uri(ServerTestUrl), router);
            listener.Start();
        }
    }
}
