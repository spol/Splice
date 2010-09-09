using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinPlexServer
{
    public class TVShow
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public TVShow(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
