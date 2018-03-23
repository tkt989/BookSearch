using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;
using Plugin.Geolocator;
using BookSearch.Service;
using BookSearch.Util;
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

            if (!isbn.IsISBN())
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await DisplayAlert("エラー", "書籍のバーコードではありません", "了解");
                    await Navigation.PopAsync(true);
                });
                return;
            }

            this.isbn = isbn;

            listView.HasUnevenRows = true;
            listView.ItemsSource = list;

            Init();
        }

        async void Init()
        {
            try
            {
                var info = await bookRepository.SearchBookInfo(isbn);

                Info.IsLoading = false;

                if (info == null)
                {
                    Info.BindingContext = new { Title = "本の情報が見つかりませんでした" };
                    return;
                }

                Info.BindingContext = info;

                if (!BookInfo.GetAll().ToList().Contains(info))
                {
                    info.Write();
                }

                var favoritesTask = bookRepository.SearchInFavorites(isbn, new Progress<List<SearchResult>>(list =>
                {
                    this.list.Favorites = list;
                }));

                var location = await CrossGeolocator.Current.GetLastKnownLocationAsync();
                if (location == null)
                {
                    location = await CrossGeolocator.Current.GetPositionAsync(TimeSpan.FromSeconds(5));
                }

                var locationTask = Task.Delay(1);
                if (location != null)
                {
                    locationTask = bookRepository.SearchInLocation(isbn, location.Longitude, location.Latitude, new Progress<List<SearchResult>>(list =>
                    {
                        this.list.Nearby = list;
                    }));
                }
                await Task.WhenAll(favoritesTask, locationTask);
            }
            catch (Plugin.Geolocator.Abstractions.GeolocationException ex)
            {
                await DisplayAlert("エラー", "位置情報を取得できませんでした", "了解");
            }
            catch (Exception ex)
            {
                await DisplayAlert("エラー", "データを取得できませんでした", "了解");
            }
        }

        public void ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (!(e.Item is SearchResult)) return;

            var result = e.Item as SearchResult;

            if (result.Status == "Running" && result.Status == "Error") return;
            Navigation.PushAsync(new SearchDetailPage(result));
        }
    }
}
