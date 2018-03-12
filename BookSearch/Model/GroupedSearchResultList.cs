using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
            favorites = new GroupedSearchResult("お気に入り");
            nearby = new GroupedSearchResult("近く");

            Add(favorites);
            Add(nearby);
        }
    }
}
