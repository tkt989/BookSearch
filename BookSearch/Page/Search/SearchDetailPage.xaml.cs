using System;
using System.Collections.Generic;

using BookSearch.Model;
using Plugin.ExternalMaps;

using Xamarin.Forms;

namespace BookSearch.Page.Search
{
    public partial class SearchDetailPage : ContentPage
    {
        SearchResult result;

        public SearchDetailPage()
        {
            InitializeComponent();
        }
        
        public SearchDetailPage(SearchResult result)
        {
            InitializeComponent();

            this.result = result;

            BindingContext = this.result;

            var index = 0;
            foreach (var pair in result.LendingList)
            {
                var lib = new Label()
                {
                    Text = pair.Key,
                };
                var status = new Label()
                {
                    Text = pair.Value,
                    HorizontalTextAlignment = TextAlignment.Center,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = GetStatusColor(pair.Value),
                };
                grid.Children.Add(lib, 0, index);
                grid.Children.Add(status, 1, index);
                index++;
            }
        }

        public async void AddressTapped(object sender, EventArgs e)
        {
            var loc = result.Geocode.Split(new char[] { ',' });
            var longitude = double.Parse(loc[0]);
            var latitude = double.Parse(loc[1]);
            var success = await CrossExternalMaps.Current.NavigateTo(result.Name, latitude, longitude);
        }

        public void URLTapped(object sender, EventArgs e)
        {
            var uri = new Uri((sender as Label)?.Text);
            Device.OpenUri(uri);
        }

        public void ReserveURLTapped(object sender, EventArgs e)
        {
            var uri = new Uri(result.ReserveURL);
            Device.OpenUri(uri);
        }

        Color GetStatusColor(string status)
        {
            switch (status)
            {
                case "貸出可":
                case "蔵書あり":
                case "館内のみ":
                    return Color.FromHex("#08b500");
                case "貸出中":
                case "予約中":
                    return Color.FromHex("#b50000");
                case "準備中":
                case "休館中":
                case "蔵書なし":
                default:
                    return Color.FromHex("#909090");
            }
        }
    }
}
