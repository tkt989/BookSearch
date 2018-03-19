using System;
using System.Linq;
using System.Collections.Generic;
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
        public string ReserveURL { get; set; } = "";
        public string Name = "";
        public string Address { get; set; } = "";
        public string URL { get; set; } = "";
        public string Geocode { get; set; }
        public string Status { get; set; } = "";

        public Dictionary<string, string> LendingList = new Dictionary<string, string>();

        public static SearchResult FromEntity(LibraryEntity entity, SearchResponse response)
        {
            return new SearchResult()
            {
                LibId = entity.LibId,
                LibKey = entity.LibKey,
                LendingList = response.LibKeyMap,
                SystemName = entity.SystemName,
                ReserveURL = response.ReserveUrl,
                Name = entity.Name,
                Address = entity.Address,
                URL = entity.URL,
                Geocode = entity.Geocode,
                Status = response.Status,
            };
        }

        public static SearchResult FromEntity(LibraryEntity entity)
        {
            return new SearchResult()
            {
                LibId = entity.LibId,
                LibKey = entity.LibKey,
                SystemName = entity.SystemName,
                ReserveURL = null,
                Name = entity.Name,
                Address = entity.Address,
                URL = entity.URL,
                Geocode = entity.Geocode,
                Status = "Running",
            };
        }

        #region ViewMode

        public string LibraryName
        {
            get => Name;
        }

        public string StatusText
        {
            get
            {
                if (Status == "Error") return "エラー";
                if (Status == "Running") return "取得中";

                var isInResponse = LendingList.Any(pair => pair.Key == LibKey);

                if (isInResponse)
                {
                    var status = LendingList.Where(pair => pair.Key == LibKey).First().Value;
                    return status;
                }

                var existsList = new List<string>() { "貸出可", "蔵書あり", "館内のみ" };
                var otherLib = LendingList.Where(pair => existsList.Contains(pair.Value));
                if (otherLib.Count() != 0) return  $"{otherLib.First().Value}(別館)";

                var failList = new List<string>() { "貸出中", "予約中" };
                otherLib = LendingList.Where(pair => failList.Contains(pair.Value));
                if (otherLib.Count() != 0) return $"{otherLib.First().Value}(別館)";

                return "蔵書なし";
            }
        }

        public Color StatusColor
        {
            get
            {
                if (Status == "Error") return Color.FromHex("#b50000");
                if (Status == "Running") return Color.FromHex("#909090");

                var isInResponse = LendingList.Any(pair => pair.Key == LibKey);

                if (isInResponse)
                {
                    var status = LendingList.Where(pair => pair.Key == LibKey).First().Value;
                    return GetStatusColor(status);
                }

                var existsList = new List<string>() { "貸出可", "蔵書あり", "館内のみ" };
                var otherLib = LendingList.Where(pair => existsList.Contains(pair.Value));
                if (otherLib.Count() != 0) return Color.FromHex("#08b500");

                var failList = new List<string>() { "貸出中", "予約中" };
                otherLib = LendingList.Where(pair => failList.Contains(pair.Value));
                if (otherLib.Count() != 0) return Color.FromHex("#b50000");

                return Color.FromHex("#909090");
            }
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

        #endregion
    }
}
