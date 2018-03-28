using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
using Realms;
using BookSearch.Service;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace BookSearch.Page
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

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
        }

		protected override void OnDisappearing()
		{
			base.OnDisappearing();

            ToolbarItems.Remove(favorite);
            ToolbarItems.Add(favorite);
		}

		void BookTapped(object sender, ItemTappedEventArgs eventArgs)
        {
            if (eventArgs.Item is Model.BookInfo info)
            {
                Navigation.PushAsync(new Search.BookSearchPage(info.ISBN));
            }
        }

        void FavoriteClicked(object sender, EventArgs eventArgs)
        {
            Navigation.PushAsync(new FavoritePage());
        }

        async void ScanClicked(object sender, EventArgs eventArgs)
        {
            //Navigation.PushAsync(new Search.BookSearchPage("4121024109"));
            var scanner = new ZXing.Mobile.MobileBarcodeScanner();
            scanner.CancelButtonText = "キャンセル";
            scanner.FlashButtonText = "フラッシュ";
            scanner.AutoFocus();
            var options = new ZXing.Mobile.MobileBarcodeScanningOptions();

            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission.Camera);

            if (status == Plugin.Permissions.Abstractions.PermissionStatus.Granted)
            {
                var result = await scanner.Scan(options);

                if (result != null)
                {
                    await Navigation.PushAsync(new Search.BookSearchPage(result.Text));
                }
            }
            else 
            {
                await DisplayAlert("エラー", "カメラを使用できません", "了解");
            }
        }
    }
}
