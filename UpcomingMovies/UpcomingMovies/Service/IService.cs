using UpcomingMovies.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UpcomingMovies.Service
{
    public interface IService
    {
        Task<TResult> Consume<TData, TResult>(TData data, string resource, HTTPMethodEnum httpMethodEnum);
    }
}
