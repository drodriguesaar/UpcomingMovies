using System.Collections.Generic;

namespace UpcomingMovies.Model
{
    public class UpcomingMovieModel : IErroModel
    {
        public UpcomingMovieModel()
        {
            Movies = new List<MovieModel>();
        }

        public int Page { get; set; }
        public int NextPage { get; set; }
        public int Total { get; set; }
        public List<MovieModel> Movies { get; set; }
        public bool Error { get; set; }
        public string ErrorMessage { get; set; }
    }
}
