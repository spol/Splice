using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinPlexServer
{
    public class VideoCollection
    {
        private string _name;
        private int _id;
        private int _type;

        public string Art
        {
            get { return "/:/resources/show-fanart.jpg"; }
        }

        public string Name
        {
            get { return _name; }
        }

        public int Id
        {
            get { return _id; }
        }

        public string Type
        {
            get
            {
                switch (_type)
                {
                    case 1:
                        return "show";
                    case 2:
                        return "movie";
                    default:
                        throw new Exception("Invalid Type");
                }
            }
        }

        public VideoCollection(int id, string name, int type)
        {
            _id = id;
            _name = name;
            _type = type;
        }
    }
}
