using System;
using System.Collections.Generic;
using System.Text;

namespace UpcomingMovies.Model
{
    public class ActorModel : IErroModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public bool Error { get; set; }
        public string ErrorMessage { get; set; }
    }
}
