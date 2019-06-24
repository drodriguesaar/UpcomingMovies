using System;
using System.Collections.Generic;
using System.Text;

namespace UpcomingMovies.DTO
{
    public class PeopleDTO
    {
        public int id { get; set; }
        public bool adult { get; set; }
        public string name { get; set; }
        public string profile_path { get; set; }
        public List<KnownForDTO> known_for { get; set; }
    }
}
