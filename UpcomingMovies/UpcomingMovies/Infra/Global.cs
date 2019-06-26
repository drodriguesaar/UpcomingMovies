using System;
using System.IO;
using UpcomingMovies.DataBase;
using UpcomingMovies.Dependency;
using UpcomingMovies.Service;
using Xamarin.Forms;

namespace UpcomingMovies.Infra
{
    public class Global
    {
        private static Global _instance;
        public static Global Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Global();
                }
                return _instance;
            }
        }

        static MovieDataBase _dataBase;
        public MovieDataBase DataBase
        {
            get
            {
                if (_dataBase == null)
                {
                    _dataBase = new MovieDataBase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MovieCache.db3"));
                }
                return _dataBase;
            }
        }

        static IToast _toast;
        public IToast Toast
        {
            get
            {
                if (_toast == null)
                {
                    _toast = DependencyService.Get<IToast>();
                }
                return _toast;
            }
        }

        static IService _baseService;
        public IService BaseService
        {
            get
            {
                if (_baseService == null)
                {
                    _baseService = ServiceFactory.GetService();
                }
                return _baseService;
            }
        }
    }
}
