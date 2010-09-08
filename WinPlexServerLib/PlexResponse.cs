using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace WinPlexServer
{
    public abstract class PlexResponse
    {
        public abstract void Send(HttpListenerResponse response);
    }
}
