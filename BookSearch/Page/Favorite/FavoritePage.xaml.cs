﻿using System.Linq;
using Realms;

using Xamarin.Forms;

namespace BookSearch.Page
{
    public partial class FavoritePage : ContentPage
    {
        public FavoritePage()
        {
            InitializeComponent();

            var favorites = Model.LibraryEntity.GetAll();
                
            favorites.SubscribeForNotifications((sender, changes, error) =>
            {
                listView.ItemsSource = sender.Select(library => library.Name);

                if (favorites.Count() == 0)
                {
                    listView.IsVisible = false;
                    message.IsVisible = true;
                    return;
                }

                listView.IsVisible = true;
                message.IsVisible = false;
            });

            listView.ItemTapped += async (sender, e) => {
                var result = await DisplayAlert("確認", "削除しますか?", "はい", "いいえ");
                if (!result) return;

                // TODO 選択した図書館を見つけるのに名前を使っているのをIdを使えるようにしたい
                var selected = favorites.Where(library => library.Name == (string)e.Item).First();
                selected.Remove();
            };
        }

        public void AddClicked(object sender, System.EventArgs eventArgs)
        {
            Navigation.PushModalAsync(new NavigationPage(new PrefListPage()));
        }
    }
}
