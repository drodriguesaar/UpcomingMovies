using System;
using System.Collections.Generic;
using System.Text;

namespace UpcomingMovies.DTO
{
    public class KnownForDTO
    {
        public string poster_path { get; set; }
        public string original_title { get; set; }
        public string popularity { get; set; }
        public int vote_count { get; set; }
        public string vote_average { get; set; }
    }
}
