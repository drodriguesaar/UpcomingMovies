namespace UpcomingMovies.Parameter
{
    public class MovieParameter
    {
        public MovieParameter()
        {
            Query = null;
            Language = null;
            Resource = null;
            Id = null;
            Page = null;
        }
        public int? Id { get; set; }
        public int? Page { get; set; }
        public string Query { get; set; }
        public string Language { get; set; }
        public string Resource { get; set; }

    }
}
