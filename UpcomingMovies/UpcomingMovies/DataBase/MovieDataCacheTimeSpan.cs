using SQLite;
using System;

namespace UpcomingMovies.DataBase
{
    public class MovieDataCacheTimeSpan
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
