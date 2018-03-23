using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
using Realms;
using BookSearch.Service;

namespace BookSearch.Page
{
    public partial class MainPage : ContentPage
    {
        CalilApi calilApi;

        public MainPage()
        {
            InitializeComponent();

            calilApi = new CalilApi();

            var favorite = new ToolbarItem();
            favorite.Text = "お気に入り";
            favorite.Clicked += (sender, e) => {
                Navigation.PushAsync(new FavoritePage());
                //Navigation.PushAsync(new FavoriteLibraryPage());
            };
            favorite.Priority = -1;

            ToolbarItems.Add(favorite);

            var resultHistroy = Model.BookInfo.GetAll();
            resultHistroy.SubscribeForNotifications((sender, changes, error) =>
            {
                listView.ItemsSource = resultHistroy.ToList();
                
                if (resultHistroy.Count() == 0)
                {
                    listView.IsVisible = false;
                    message.IsVisible = true;
                    return;
                }

                listView.IsVisible = true;
                message.IsVisible = false;
            });

            scanButton.Clicked += async (sender, e) => {
                //Navigation.PushAsync(new Search.BookSearchPage("4121024109"));
                var scanner = new ZXing.Mobile.MobileBarcodeScanner();
                var options = new ZXing.Mobile.MobileBarcodeScanningOptions();
                var result = await scanner.Scan(options);

                if (result != null) {
                    await Navigation.PushAsync(new Search.BookSearchPage(result.Text));
                }
            };
        }

        private void BookTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is Model.BookInfo info)
            {
                Navigation.PushAsync(new Search.BookSearchPage(info.ISBN));
            }
        }
    }
}
