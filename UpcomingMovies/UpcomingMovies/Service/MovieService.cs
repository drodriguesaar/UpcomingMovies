using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UpcomingMovies.Consts;
using UpcomingMovies.DTO;
using UpcomingMovies.Enums;
using UpcomingMovies.Infra;
using UpcomingMovies.Model;
using UpcomingMovies.Parameter;

namespace UpcomingMovies.Component
{
    public class MovieService
    {
        internal async Task<List<MovieModel>> GetMovies(MovieParameter movieParameter)
        {
            var moviesList = new List<MovieModel>();
            try
            {
                var resource = movieParameter.Resource;
                var response = await Global.Instance.BaseService.Consume<MovieParameter, ResponseListDTO<List<MovieDTO>>>(movieParameter, resource, HTTPMethodEnum.GET);
                moviesList = response.results.Select(movie => new MovieModel
                {
                    ReleaseDate = string.IsNullOrEmpty(movie.release_date) ? "not available" : DateTime.Parse(movie.release_date).ToShortDateString(),
                    Name = movie.title,
                    OverView = BuildAbreviatedMovieOverView(movie.overview),
                    Poster = movie.poster_path.BuildImageURI(),
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
            MovieModel movieModel = new MovieModel();
            try
            {
                var movieresource = string.Format(MoviesApiResourcesConsts.MOVIE, movieParameter.Id);
                var movieID = movieParameter.Id;
                movieParameter.Id = null;
                var response = await Global.Instance.BaseService.Consume<MovieParameter, MovieDTO>(movieParameter, movieresource, HTTPMethodEnum.GET);

                var releaseDate = new DateTime();
                DateTime.TryParse(response.release_date, out releaseDate);
                var releaseDateFormatted = releaseDate == DateTime.MinValue ? string.Empty : releaseDate.ToShortDateString();

                var genres = response.genres == null ? "Not available" : string.Join(", ", response.genres.Select(g => g.name));

                movieModel.ReleaseDate = releaseDateFormatted;
                movieModel.Name = response.title;
                movieModel.OverView = response.overview;
                movieModel.Genres = genres;
                movieModel.Poster = response.poster_path.BuildImageURI(size: "500");
                movieModel.Id = response.id;
                movieModel.Score = response.vote_average;
                movieModel.Votes = response.vote_count;
                movieModel.Language = response.original_language;
                movieModel.HomePage = response.homepage;

                movieresource = string.Format(MoviesApiResourcesConsts.MOVIE_IMAGES, movieID);
                var imagesresponse = await Global.Instance.BaseService.Consume<MovieParameter, MovieDTO>(movieParameter, movieresource, HTTPMethodEnum.GET);
                movieModel.Images.AddRange(imagesresponse.posters.Select(p => new ImageModel
                {
                    Height = p.height,
                    Width = p.width,
                    Path = p.file_path.BuildImageURI()
                }));
            }
            catch (Exception ex)
            {
                movieModel.Error = true;
                movieModel.ErrorMessage = ex.Message;
            }
            finally
            {

            }
            return movieModel;
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
