using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpcomingMovies.Consts;
using UpcomingMovies.DTO;
using UpcomingMovies.Enums;
using UpcomingMovies.Model;
using UpcomingMovies.Parameter;

namespace UpcomingMovies.Service
{
    public class GenreService
    {
        public GenreService()
        {

        }

        internal async Task<List<GenreModel>> GetGenres()
        {
            var genresList = new List<GenreModel>();

            try
            {
                var movieParameter = new MovieParameter();
                var resource = MoviesApiResourcesConsts.GENRES;
                movieParameter.Resource = null;
                var response = await App.BaseService.Consume<MovieParameter, ResponseListDTO<List<GenreDTO>>>(movieParameter, resource, HTTPMethodEnum.GET);
                genresList = response.genres.Select(genre => new GenreModel
                {
                    ID = genre.id,
                    Name = genre.name
                }).ToList();
            }
            catch (Exception ex)
            {
            }
            return genresList;
        }
    }
}
