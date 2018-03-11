using System;
using System.Collections.Generic;

using BookSearch.Model;

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
        }

        public void ReserveURLTapped(object sender, EventArgs e)
        {
            var uri = new Uri(result.ReserveURL);
            Device.OpenUri(uri);
        }
    }
}
