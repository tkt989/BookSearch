using System;
using System.Linq;
using BookSearch.Model;
using BookSearch.Service;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace BookSearch.Page
{
    public partial class LibraryListPage : ContentPage
    {
        public LibraryListPage(string pref, string city)
        {

            InitializeComponent();

            UpdateLibraries(pref, city);
        }

        async void UpdateLibraries(string pref, string city)
        {
            var calilApi = new CalilApi();
            var libraries = await calilApi.GetLibraryAsync(pref, city);

            listView.ItemsSource = libraries.Select(library => library.Name);
            listView.ItemTapped += async (sender, e) =>
            {
                var favorites = LibraryEntity.GetAll().ToList();
                var selected = libraries.Where(library => library.Name == (string)e.Item).First();

                if (favorites.Contains(selected))
                {
                    await DisplayAlert("確認", "登録済みです", "閉じる");
                    return;
                }

                var yes = await DisplayAlert("登録", String.Format("{0}を登録しますか?", selected.Name), "はい", "いいえ");
                if (yes)
                {
                    selected.Write();
                    await Navigation.PopModalAsync(true);
                }
            };
        }
    }
}
