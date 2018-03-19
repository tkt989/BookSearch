using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

        public virtual void ReplaceAll(IEnumerable<SearchResult> collection)
        {
            this.Items.Clear();

            foreach (SearchResult item in collection)
            {
                this.Items.Add(item);
            }

            this.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            this.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            // Cannot use NotifyCollectionChangedAction.Add, because Constructor supports only the 'Reset' action.
        }    }
}
