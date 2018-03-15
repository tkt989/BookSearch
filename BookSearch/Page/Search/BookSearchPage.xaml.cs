using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;
using Plugin.Geolocator;
using BookSearch.Service;
using System.Runtime.CompilerServices;
using BookSearch.Model;

namespace BookSearch.Page.Search
{
    public partial class BookSearchPage : ContentPage
    {
        private string isbn;
        GroupedSearchResultList list = new GroupedSearchResultList();
        BookRepository bookRepository;

        public BookSearchPage()
        {
            InitializeComponent();
        }

        public BookSearchPage(string isbn) : this()
        {
            bookRepository = new BookRepository();
            this.isbn = isbn;

            listView.HasUnevenRows = true;
            listView.ItemsSource = list;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var location = await CrossGeolocator.Current.GetPositionAsync();
                    await bookRepository.SearchInLocation(isbn, location.Longitude, location.Latitude, list =>
                    {
                        this.list.Nearby = list;
                    });
                });
            });
            
            Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                    {
                        await bookRepository.SearchInFavorites(isbn, list =>
                        {
                            this.list.Favorites = list;
                        });
                    });
            });

            Task.Run(async () =>
            {
                var info = await bookRepository.SearchBookInfo(isbn);

                Device.BeginInvokeOnMainThread(() =>
                {
                    if (info == null)
                    {
                        Title.Text = "本の情報が見つかりませんでした。";
                        Thumbnail.Source = null;
                        return;
                    }

                    Title.Text = info.Title;
                    Thumbnail.Source = new UriImageSource() { Uri = new Uri(info.Thumbnail) };

                    if (!BookInfo.GetAll().ToList().Contains(info))
                    {
                        info.Write();
                    }
                });
            });
        }

        public void ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (!(e.Item is SearchResult)) return;

            var result = e.Item as SearchResult;
            Navigation.PushAsync(new SearchDetailPage(result));
        }
    }
}
