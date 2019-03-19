
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UpcomingMovies.Consts;
using UpcomingMovies.DTO;
using UpcomingMovies.Enums;
using UpcomingMovies.Model;
using UpcomingMovies.Parameter;

namespace UpcomingMovies.Component
{
    public class MovieService
    {

        public MovieService()
        {

        }
        internal async Task<List<MovieModel>> GetMovies(MovieParameter movieParameter)
        {
            var moviesList = new List<MovieModel>();

            try
            {
                var resource = movieParameter.Resource;
                movieParameter.Resource = null;
                var response = await App.BaseService.Consume<MovieParameter, ResponseListDTO<List<MovieDTO>>>(movieParameter, resource, HTTPMethodEnum.GET);
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
            catch (Exception ex)
            {
            }
            return moviesList;
        }
        internal async Task<MovieModel> GetMovie(MovieParameter movieParameter)
        {
            var movieresource = string.Format(MoviesApiResourcesConsts.MOVIE, movieParameter.Id);
            movieParameter.Id = null;
            MovieModel movieModel = new MovieModel();
            try
            {
                var response = await App.BaseService.Consume<MovieParameter, MovieDTO>(movieParameter, movieresource, HTTPMethodEnum.GET);

                var releaseDate = new DateTime();
                DateTime.TryParse(response.release_date, out releaseDate);
                var releaseDateFormatted = releaseDate == DateTime.MinValue ? string.Empty : releaseDate.ToShortDateString();

                var genres = response.genres == null ? "Not available" : string.Join(", ", response.genres.Select(g => g.name));

                movieModel.ReleaseDate = releaseDateFormatted;
                movieModel.Name = response.title;
                movieModel.OverView = response.overview;
                movieModel.Genres = genres;
                movieModel.Poster = BuildPosterUri(response.poster_path, "500");
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
