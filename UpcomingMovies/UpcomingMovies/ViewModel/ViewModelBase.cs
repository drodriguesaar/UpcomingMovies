using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace UpcomingMovies.ViewModel
{
   public class ViewModelBase : INotifyPropertyChanged
    {
        protected bool _Navigated { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
