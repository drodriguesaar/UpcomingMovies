using System.Collections.Generic;

namespace UpcomingMovies.DTO
{
    internal class MovieDTO
    {

        public MovieDTO()
        {
            production_companies = new List<CompanyDTO>();
            production_countries = new List<CountryDTO>();
        }

        public string backdrop_path { get; set; }
        public List<int> genre_ids { get; set; }
        public int id { get; set; }
        public string original_title { get; set; }
        public string overview { get; set; }
        public string poster_path { get; set; }
        public string release_date { get; set; }
        public string title { get; set; }
        public string homepage { get; set; }
        public string popularity { get; set; }
        public string vote_average { get; set; }
        public string vote_count { get; set; }
        public string revenue { get; set; }
        public string status { get; set; }
        public string imdb_id { get; set; }
        public bool adult { get; set; }
        public string original_language { get; set; }
        public List<CompanyDTO> production_companies { get; set; }
        public List<CountryDTO> production_countries { get; set; }
        public List<GenreDTO> genres { get; set; }
    }
}
