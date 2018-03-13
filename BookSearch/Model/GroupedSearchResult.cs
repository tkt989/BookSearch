using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace BookSearch.Model
{
    public class GroupedSearchResult : ObservableCollection<SearchResult>
    {
        public string Title { get; set; }
        public ImageSource Icon { get; set; }

        public GroupedSearchResult(string title, ImageSource icon)
        {
            Title = title;
            Icon = icon;
        }
    }
}
