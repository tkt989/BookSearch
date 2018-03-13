using System;
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
                favorites.Clear();
                foreach (var v in value)
                {
                    favorites.Add(v);
                }
            }
        }

        GroupedSearchResult nearby;
        public List<SearchResult> Nearby
        {
            set
            {
                nearby.Clear();
                foreach (var v in value)
                {
                    nearby.Add(v);
                }
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
