using System;
using System.Collections.Generic;
using System.Text;

namespace UpcomingMovies.Model
{
    public class ActorModel : IErroModel
    {
        public ActorModel()
        {
            KnownFor = new List<KnownForModel>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public string Department { get; set; }
        public string POB { get; set; }
        public string DOB { get; set; }
        public string DOD { get; set; }
        public string Bio { get; set; }
        public string Adult { get; set; }
        public string Gender { get; set; }
        public bool Error { get; set; }
        public int Position { get; set; }
        public string ErrorMessage { get; set; }
        public string HomePage { get; set; }
        public string Popularity { get; set; }
        public List<KnownForModel> KnownFor { get; set; }
    }
}
