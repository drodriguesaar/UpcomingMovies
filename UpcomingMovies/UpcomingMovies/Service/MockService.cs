using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UpcomingMovies.Consts;
using UpcomingMovies.DTO;
using UpcomingMovies.Enums;
using UpcomingMovies.Parameter;

namespace UpcomingMovies.Service
{
    internal class MockService : IService
    {
        List<MovieDTO> Movies { get; set; }

        public MockService()
        {
            Movies = GetMovies();
        }

        List<MovieDTO> GetMovies()
        {
            return new List<MovieDTO>
            {
                new MovieDTO
                {
                    vote_count = "166428",
                    id = 166428,
                    vote_average = "7.8",
                    title = "How to Train Your Dragon: The Hidden World 1",
                    popularity = "582.873",
                    poster_path ="/xvx4Yhf0DVH8G4LzNISpMfFBDy2.jpg",
                    original_language = "en",
                    original_title = "How to Train Your Dragon: The Hidden World",
                    genre_ids = new List<int>{ 16, 10751, 12 },
                    backdrop_path = "/h3KN24PrOheHVYs9ypuOIdFBEpX.jpg",
                    adult = false,
                    overview="As Hiccup fulfills his dream of creating a peaceful dragon utopia, Toothless’ discovery of an untamed, elusive mate draws the Night Fury away. When danger mounts at home and Hiccup’s reign as village chief is tested, both dragon and rider must make impossible decisions to save their kind.",
                    release_date="2019-01-03"
                },
                new MovieDTO
                {
                    vote_count = "72",
                    id = 450001,
                    vote_average = "5.6",
                    title = "Master Z: Ip Man Legacy",
                    popularity = "526.566",
                    poster_path ="/2WfjB6FUDTIBVI2y02UGbnHR82s.jpg",
                    original_language = "cn",
                    original_title = "葉問外傳：張天志",
                    genre_ids = new List<int>{ 28 },
                    backdrop_path = "/grtVFGJ4ts0nDAPpc1JWbBoVKTu.jpg",
                    adult = false,
                    overview="After being defeated by Ip Man, Cheung Tin Chi is attempting to keep a low profile. While going about his business, he gets into a fight with a foreigner by the name of Davidson, who is a big boss behind the bar district. Tin Chi fights hard with Wing Chun and earns respect",
                    release_date="2018-12-20"
                },
                new MovieDTO
                {
                    vote_count = "6248",
                    id = 424694,
                    vote_average = "8.1",
                    title = "Bohemian Rhapsody",
                    popularity = "118.718",
                    poster_path ="/lHu1wtNaczFPGFDTrjCSzeLPTKN.jpg",
                    original_language = "en",
                    original_title = "Bohemian Rhapsody",
                    genre_ids = new List<int>{ 18, 10402 },
                    backdrop_path = "/xcaSYLBhmDzJ6P14bcKe0KTh3QV.jpg",
                    adult = false,
                    overview="Singer Freddie Mercury, guitarist Brian May, drummer Roger Taylor and bass guitarist John Deacon take the music world by storm when they form the rock 'n' roll band Queen in 1970. Hit songs become instant classics. When Mercury's increasingly wild lifestyle starts to spiral out of control, Queen soon faces its greatest challenge yet – finding a way to keep the band together amid the success and excess.",
                    release_date="2018-10-24"
                }
            };
        }

        public async Task<TResult> Consume<TData, TResult>(TData data, string resource, HTTPMethodEnum httpMethodEnum)
        {
            await Task.Delay(2000);
            var parameter = (MovieParameter)(object)data;
            if (resource == MoviesApiResourcesConsts.UPCOMING_MOVIES)
            {
                return (TResult)(object)(new ResponseListDTO<List<MovieDTO>>()
                {
                    results = Movies,
                    page = parameter.Id.GetValueOrDefault(),
                    total_results = Movies.Count,
                    total_pages = 1
                });
            }

            var movieId = resource.Split('/').Last();
            var id = 0;
            int.TryParse(movieId, out id);
            return (TResult)(object)Movies.SingleOrDefault(movie => movie.id == id);
        }
    }
}
