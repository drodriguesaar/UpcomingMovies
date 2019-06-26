using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpcomingMovies.Consts;
using UpcomingMovies.DTO;
using UpcomingMovies.Enums;
using UpcomingMovies.Infra;
using UpcomingMovies.Model;
using UpcomingMovies.Parameter;

namespace UpcomingMovies.Service
{
    public class GenreService
    {
        internal async Task<List<GenreModel>> GetGenres(MovieParameter movieParameter)
        {
            var genresList = new List<GenreModel>();
            try
            {
                var resource = MoviesApiResourcesConsts.GENRES;
                var response = await Global.Instance.BaseService.Consume<MovieParameter, ResponseListDTO<List<GenreDTO>>>(movieParameter, resource, HTTPMethodEnum.GET);
                genresList = response.genres.Select(genre => new GenreModel
                {
                    ID = genre.id,
                    Name = genre.name
                }).ToList();
            }
            catch
            {
            }
            return genresList;
        }
    }
}
