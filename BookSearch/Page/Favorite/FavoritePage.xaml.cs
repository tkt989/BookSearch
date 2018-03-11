using System.Linq;
using Realms;

using Xamarin.Forms;

namespace BookSearch.Page
{
    public partial class FavoritePage : ContentPage
    {
        public FavoritePage()
        {
            InitializeComponent();

            SetupToolbar();

            var favorites = Model.LibraryEntity.GetAll();
                
            favorites.SubscribeForNotifications((sender, changes, error) =>
            {
                listView.ItemsSource = sender.Select(library => library.Name);
            });

            listView.ItemTapped += async (sender, e) => {
                var result = await DisplayAlert("確認", "削除しますか?", "はい", "いいえ");
                if (!result) return;

                // TODO 選択した図書館を見つけるのに名前を使っているのをIdを使えるようにしたい
                var selected = favorites.Where(library => library.Name == (string)e.Item).First();
                selected.Remove();
            };
        }

        void SetupToolbar()
        {
            var add = new ToolbarItem();
            add.Text = "追加";
            add.Clicked += (sender, e) => Navigation.PushModalAsync(new NavigationPage(new PrefListPage()));
            ToolbarItems.Add(add);;
        }
    }
}
