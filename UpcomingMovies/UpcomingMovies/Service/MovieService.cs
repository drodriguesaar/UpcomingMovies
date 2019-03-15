
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UpcomingMovies.Consts;
using UpcomingMovies.DTO;
using UpcomingMovies.Enums;
using UpcomingMovies.Model;
using UpcomingMovies.Parameter;
using UpcomingMovies.Service;

namespace UpcomingMovies.Component
{
    public class MovieService
    {
        IService _baseService;
        public MovieService()
        {

        }
        public MovieService(IService service)
        {
            _baseService = service;
        }
        public string Resource { get; set; }
        internal async Task<List<MovieModel>> GetMovies(MovieParameter movieParameter)
        {
            _baseService = new BaseService(Resource);
            var moviesList = new List<MovieModel>();
            try
            {
                var response = await _baseService.Consume<MovieParameter, ResponseListDTO<List<MovieDTO>>>(movieParameter, HTTPMethodEnum.GET);
                moviesList = response.results.Select(movie => new MovieModel
                {
                    ReleaseDate = string.IsNullOrEmpty(movie.release_date) ? "not available" : DateTime.Parse(movie.release_date).ToShortDateString(),
                    Name = movie.title,
                    OverView = BuildAbreviatedMovieOverView(movie.overview),
                    Poster = BuildPosterUri(movie.poster_path),
                    Score = movie.vote_average,
                    Votes = movie.vote_count,
                    Id = movie.id
                }).ToList();
            }
            catch
            {
            }
            return moviesList;
        }
        internal async Task<MovieModel> GetMovie(MovieParameter movieParameter)
        {
            var movieresource = string.Format(MoviesApiResourcesConsts.MOVIE, movieParameter.Id);
            movieParameter.Id = null;
            _baseService = new BaseService(movieresource);
            MovieModel movieModel = new MovieModel();
            try
            {
                var response = await _baseService.Consume<MovieParameter, MovieDTO>(movieParameter, HTTPMethodEnum.GET);
                var releaseDate = new DateTime();
                DateTime.TryParse(response.release_date, out releaseDate);
                var releaseDateFormatted = releaseDate == DateTime.MinValue ? string.Empty : DateTime.Parse(response.release_date).ToShortDateString();

                movieModel.ReleaseDate = releaseDateFormatted;
                movieModel.Name = response.title;
                movieModel.OverView = response.overview;
                movieModel.Genres = string.Join(", ", response.genres.Select(g => g.name));
                movieModel.Poster = BuildPosterUri(response.poster_path,"500");
                movieModel.Id = response.id;
                movieModel.Score = response.vote_average;
                movieModel.Votes = response.vote_count;
                movieModel.Language = response.original_language;
                movieModel.HomePage = response.homepage;
            }
            catch (Exception ex)
            {
                movieModel.Error = true;
                movieModel.ErrorMessage = ex.Message;
            }
            return movieModel;
        }
        string BuildPosterUri(string posterPath, string size = "200")
        {
            if (string.IsNullOrEmpty(posterPath))
            {
                return "https://tinyurl.com/y2eejo2m";
            }

            return string.Format("https://image.tmdb.org/t/p/w{1}/{0}", posterPath, size);
        }
        string BuildAbreviatedMovieOverView(string movieDescription)
        {
            if (string.IsNullOrEmpty(movieDescription))
            {
                return string.Empty;
            }

            var words = movieDescription.Split(' ').Take(15);
            var abreviatedOverView = string.Join(" ", words);
            abreviatedOverView = string.Concat(abreviatedOverView, "...");

            return abreviatedOverView;
        }
    }
}
