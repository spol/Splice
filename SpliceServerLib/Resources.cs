using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinPlexServer
{
    class Resources : IController
    {
        public PlexResponse HandleRequest(PlexRequest request)
        {
            return XmlResponse.NotFound();
        }

    }
}
