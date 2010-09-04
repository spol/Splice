using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace WinPlexServer
{
    public class Section
    {
        public virtual void HandleRequest(HttpListenerResponse response, string path)
        {
        }
    }

}
