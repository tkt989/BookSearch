using System;
using System.Linq;
using System.Collections.Generic;
using BookSearch.Model;
using Xamarin.Forms;
using System.Threading.Tasks;
using BookSearch.Service;

namespace BookSearch.Page
{
    public partial class CityListPage : ContentPage
    {
        public CityListPage(string pref)
        {
            InitializeComponent();

            listView.ItemTapped += (sender, e) => {
                var city = (string)e.Item;
                Navigation.PushAsync(new LibraryListPage(pref, city));
            };

            UpdateLibraries(pref);
        }

        async Task UpdateLibraries(string pref)
        {
            var calilApi = new CalilApi();
            var libraries = await calilApi.GetLibraryAsync(pref);

            listView.ItemsSource = libraries.Select(library => library.City).Distinct().OrderBy(library => library);
            return;
        }
    }
}
