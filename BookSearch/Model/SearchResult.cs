using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace BookSearch.Model
{
    public enum BookStatus
    {
        AVAILABLE,
        OTHER_LIBRARY,
        LOADING,
        NOT_AVAILABLE,
        ERROR,
    }

    public class SearchResult : ViewModelBase
    {
        public string LibId = "";
        public string LibKey = "";
        public string SystemName = "";
        public string ReserveURL = "";
        public string Name = "";
        public string Address { get; set; } = "";

        public BookStatus BookStatus;
        public Dictionary<string, BookStatus> LendingList = new Dictionary<string, BookStatus>();

        public static SearchResult FromEntity(LibraryEntity entity, SearchResponse response)
        {
            var available = response.LibKeyMap.Any(pair => pair.Key == entity.LibKey);

            var status = BookStatus.NOT_AVAILABLE;
            if (available)
            {
                status = BookStatus.AVAILABLE;
            }

            if (response.Status == "Running") status = BookStatus.LOADING;
            if (response.Status == "Error") status = BookStatus.ERROR;

            return new SearchResult()
            {
                LibId = entity.LibId,
                LibKey = entity.LibKey,
                SystemName = entity.SystemName,
                ReserveURL = response.ReserveUrl,
                Name = entity.Name,
                BookStatus = status,
                Address = entity.Address,
            };
        }

        #region ViewMode

        public string LibraryName
        {
            get => Name;
        }

        public string Status
        {
            get
            {
                switch (BookStatus)
                {
                    case BookStatus.AVAILABLE:
                    case BookStatus.OTHER_LIBRARY:
                        return "貸出可";
                    case BookStatus.NOT_AVAILABLE:
                        return "貸出不可";
                    case BookStatus.LOADING:
                        return "読み込み中";
                    default:
                        return "エラー";
                }
            }
        }

        #endregion
    }
}
