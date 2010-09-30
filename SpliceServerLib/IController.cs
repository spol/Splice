using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Splice.Server
{
    public interface IController
    {
        PlexResponse HandleRequest(PlexRequest request);
    }
}
