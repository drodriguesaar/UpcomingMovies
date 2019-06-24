using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using UpcomingMovies.Model;
using Xamarin.Forms;

namespace UpcomingMovies.CustomViews
{
    public class GenresStackLayout : StackLayout
    {
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create("ItemsSource", typeof(ObservableCollection<GenreModel>), typeof(GenresStackLayout), default(ObservableCollection<GenreModel>),
                propertyChanged: OnItemsSourceChanged);

        public static readonly BindableProperty ItemTemplateProperty =
            BindableProperty.Create("ItemTemplate", typeof(DataTemplate), typeof(GenresStackLayout), default(DataTemplate));

        public ObservableCollection<GenreModel> ItemsSource
        {
            set { SetValue(ItemsSourceProperty, value); }
            get { return (ObservableCollection<GenreModel>)GetValue(ItemsSourceProperty); }
        }

        public DataTemplate ItemTemplate
        {
            set { SetValue(ItemTemplateProperty, value); }
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
        }

        public GenresStackLayout()
        {
            SetItemsSourceData((ObservableCollection<GenreModel>)ItemsSourceProperty.DefaultValue);
        }

        static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((GenresStackLayout)bindable).OnItemsSourceChanged((ObservableCollection<GenreModel>)oldValue, (ObservableCollection<GenreModel>)newValue);
        }

        void OnItemsSourceChanged(ObservableCollection<GenreModel> oldValue, ObservableCollection<GenreModel> newValue)
        {
            SetItemsSourceData(newValue);
        }
        void SetItemsSourceData(ObservableCollection<GenreModel> genres)
        {
            this.ItemsSource = genres;
        }
    }
}
