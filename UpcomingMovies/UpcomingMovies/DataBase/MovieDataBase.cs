using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UpcomingMovies.DTO;
using UpcomingMovies.Model;

namespace UpcomingMovies.DataBase
{
    public class MovieDataBase
    {
        readonly SQLiteAsyncConnection _dataBase;

        public MovieDataBase(string dbPath)
        {
            _dataBase = new SQLiteAsyncConnection(dbPath);
            _dataBase.CreateTableAsync<MovieModel>().Wait();
            _dataBase.CreateTableAsync<MovieDataCacheTimeSpan>().Wait();
        }

        /// <summary>
        /// Get movie cache timespan on that day.
        /// </summary>
        /// <returns></returns>
        public async Task<MovieDataCacheTimeSpan> GetMovieDataCacheTimeSpanAsync()
        {
            return await _dataBase.Table<MovieDataCacheTimeSpan>().FirstOrDefaultAsync();
        }

        /// <summary>
        /// Update movie cache timespan.
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        public async Task<int> UpdateMovieDataCacheTimeSpanAsync(DateTime dateFrom, DateTime dateTo)
        {
            var movieDataCacheTimeSpan = await _dataBase.Table<MovieDataCacheTimeSpan>().FirstOrDefaultAsync();
            movieDataCacheTimeSpan.DateFrom = dateFrom;
            movieDataCacheTimeSpan.DateTo = dateTo;
            return await _dataBase.UpdateAsync(movieDataCacheTimeSpan);
        }

        public async Task<int> InsertMovieDataCacheTimeSpanAsync(DateTime dateFrom, DateTime dateTo)
        {
            return await _dataBase.InsertAsync(new MovieDataCacheTimeSpan { DateFrom = dateFrom, DateTo = dateTo });
        }

        /// <summary>
        /// Get movies cached.
        /// </summary>
        /// <returns></returns>
        public async Task<List<MovieModel>> GetMoviesAsync()
        {
            return await _dataBase.Table<MovieModel>().ToListAsync();
        }

        /// <summary>
        /// Insert new movies to be cached.
        /// </summary>
        /// <param name="movies"></param>
        /// <returns></returns>
        public async Task<int> InsertMoviesAsync(List<MovieModel> movies)
        {
            return await _dataBase.InsertAllAsync(movies);
        }

        /// <summary>
        /// Delete existing movies cached.
        /// </summary>
        /// <returns></returns>
        public async Task<int> DeleteMoviesAsync()
        {
            return await _dataBase.DeleteAllAsync<MovieModel>();
        }
    }
}
