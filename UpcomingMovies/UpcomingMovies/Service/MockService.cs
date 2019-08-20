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
                },
                new MovieDTO
                {
                    vote_count = "11",
                    id = 283552,
                    vote_average = "4.41",
                    title = "The Light Between Oceans",
                    popularity = "4.546151",
                    poster_path ="/pEFRzXtLmxYNjGd0XqJDHPDFKB2.jpg",
                    original_language = "en",
                    original_title = "The Light Between Oceans",
                    genre_ids = new List<int>{ 18 },
                    backdrop_path = "/2Ah63TIvVmZM3hzUwR5hXFg2LEk.jpg",
                    adult = false,
                    overview="A lighthouse keeper and his wife living off the coast of Western Australia raise a baby they rescue from an adrift.",
                    release_date="2018-10-24"
                },
                new MovieDTO
                {
                    vote_count = "97",
                    id = 342521,
                    vote_average = "6.04",
                    title = "Keanu",
                    popularity = "3.51555",
                    poster_path ="/udU6t5xPNDLlRTxhjXqgWFFYlvO.jpg",
                    original_language = "en",
                    original_title = "Keanu",
                    genre_ids = new List<int>{ 28,35 },
                    backdrop_path = "/scM6zcBTXvUByKxQnyM11qWJbtX.jpg",
                    adult = false,
                    overview="Friends hatch a plot to retrieve a stolen cat by posing as drug dealers for a street gang.",
                    release_date="2016-09-14"
                },
                new MovieDTO
                {
                    vote_count = "8",
                    id = 363676,
                    vote_average = "4.88",
                    title = "Sully",
                    popularity = "3.254896",
                    poster_path ="/1BdD1kMK1phbANQHmddADzoeKgr.jpg",
                    original_language = "en",
                    original_title = "Sully",
                    genre_ids = new List<int>{ 36, 18 },
                    backdrop_path = "/nfj8iBvOjlb7ArbThO764HCQw5H.jpg",
                    adult = false,
                    overview="On January 15, 2009, the world witnessed the \"Miracle on the Hudson\" when Captain \"Sully\" Sullenberger glided his disabled plane onto the frigid waters of the Hudson River, saving the lives of all 155 aboard. However, even as Sully was being heralded by the public and the media for his unprecedented feat of aviation skill, an investigation was unfolding that threatened to destroy his reputation and his career.",
                    release_date="2016-09-08"
                }
            };
        }

        public async Task<TResult> Consume<TData, TResult>(TData data, string resource, HTTPMethodEnum httpMethodEnum)
        {
            await Task.Delay(2000);
            var parameter = (MovieParameter)(object)data;

            switch (resource)
            {
                case MoviesApiResourcesConsts.UPCOMING_MOVIES:
                    return (TResult)(object)(new ResponseListDTO<List<MovieDTO>>()
                    {
                        results = Movies,
                        page = parameter.Id.GetValueOrDefault(),
                        total_results = Movies.Count,
                        total_pages = 1
                    });
                case MoviesApiResourcesConsts.SEARCH_MOVIE:
                    return (TResult)(object)(new ResponseListDTO<List<MovieDTO>>()
                    {
                        results = Movies.Where(m => m.title.ToLower().Contains(parameter.Query.ToLower())).ToList(),
                        page = parameter.Id.GetValueOrDefault(),
                        total_results = Movies.Count,
                        total_pages = 1
                    });
                default:
                    var movieId = resource.Split('/').Last();
                    var id = 0;
                    int.TryParse(movieId, out id);
                    return (TResult)(object)Movies.SingleOrDefault(movie => movie.id == id);
            }
        }
    }
}
