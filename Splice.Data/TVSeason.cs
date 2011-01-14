using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Splice.Data
{
    public class TVSeason
    {
        public Int32 Id { get; set; }
        public string Title
        {
            get
            {
                if (SeasonNumber == 0)
                {
                    return "Specials";
                }
                else
                {
                    return "Season " + SeasonNumber.ToString();
                }
            }
        }
        public Int32 SeasonNumber { get; set; }
        public Int32 ShowId { get; set; }
        public string Art { get; set; }

        public TVSeason() { }

        public TVSeason(DataRow Row)
        {
            Id = Convert.ToInt32(Row["id"]);
            SeasonNumber = Convert.ToInt32(Row["seasonNumber"]);
            ShowId = Convert.ToInt32(Row["showId"]);
            Art = Row["art"].ToString();
        }
    }
}
