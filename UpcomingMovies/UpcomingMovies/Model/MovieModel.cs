using SQLite;
using System.Collections.Generic;

namespace UpcomingMovies.Model
{
    public class MovieModel : IErroModel
    {
        public int Id { get; set; }
        public int Position { get; set; }
        public string Name { get; set; }
        public string Poster { get; set; }
        public string Genres { get; set; }
        public string OverView { get; set; }
        public string ReleaseDate { get; set; }
        public bool Error { get; set; }
        public string ErrorMessage { get; set; }
        public string ProducedBy { get; set; }
        public string ProducedIn { get; set; }
        public string Score { get; set; }
        public string Votes { get; set; }
        public string Revenue { get; set; }
        public string Language { get; set; }
        public string OriginalName { get; set; }
        public string HomePage { get; set; }
        [Ignore]
        public List<ImageModel> Images { get; set; }
    }
}
