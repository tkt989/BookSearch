using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
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

            ToolbarItems.Add(favorite);

            scanButton.Clicked += async (sender, e) => {
                Navigation.PushAsync(new Search.BookSearchPage("4121024109"));
                //var scanner = new ZXing.Mobile.MobileBarcodeScanner();
                //var options = new ZXing.Mobile.MobileBarcodeScanningOptions();
                //var result = await scanner.Scan(options);

                //if (result != null) {
                //    await Navigation.PushAsync(new Search.BookSearchPage(result.Text));
                //}
            };
        }
    }
}
