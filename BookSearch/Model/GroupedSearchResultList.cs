using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace BookSearch.Model
{
    public class GroupedSearchResultList : ObservableCollection<GroupedSearchResult>
    {
        GroupedSearchResult favorites;
        public List<SearchResult> Favorites
        {
            set
            {
                favorites.ReplaceAll(value);
            }
        }

        void Update(ref SearchResult from, SearchResult to)
        {
            from = to;
        }

        GroupedSearchResult nearby;
        public List<SearchResult> Nearby
        {
            set
            {
                nearby.ReplaceAll(value);
            }
        }
        
        public GroupedSearchResultList()
        {
            favorites = new GroupedSearchResult("お気に入り", ImageSource.FromResource("BookSearch.Resources.star.png"));
            nearby = new GroupedSearchResult("近く", ImageSource.FromResource("BookSearch.Resources.location.png"));

            Add(favorites);
            Add(nearby);
        }
    }
}
