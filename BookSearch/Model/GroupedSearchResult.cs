using System;
using System.Collections.ObjectModel;

namespace BookSearch.Model
{
    public class GroupedSearchResult : ObservableCollection<SearchResult>
    {
        public string Title { get; set; }

        public GroupedSearchResult(string title)
        {
            Title = title;
        }
    }
}
