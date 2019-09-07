using System;
using System.Collections.Generic;
using System.Text;

namespace UpcomingMovies.Dependency
{
    public interface ICameraOCR
    {
        string ReadTextFromImage(string file_path);

    }
}
