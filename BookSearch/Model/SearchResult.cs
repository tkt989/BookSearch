using System;
using System.Linq;
using System.Collections.Generic;
using Xamarin.Forms;

namespace BookSearch.Model
{
    public class SearchResult : ViewModelBase
    {
        static readonly List<string> AVAILABLE = new List<string>
        {
            "貸出可", "蔵書あり", "館内のみ"
        };
        static readonly List<string> NOT_AVAILABLE = new List<string>
        {
            "貸出中", "予約中"
        };
        
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

                var status = GetDisplayStatus();

                if (!status.HasValue) return "蔵書なし";

                if (status.Value.Key == LibKey)
                {
                    return status.Value.Value;
                }

                return $"{status.Value.Value}(別館)";
            }
        }

        public Color StatusColor
        {
            get
            {
                var statusText = StatusText;

                if (AVAILABLE.Any(text => statusText.Contains(text)))
                {
                    return Color.FromHex("#08b500");
                }

                if (NOT_AVAILABLE.Any(text => statusText.Contains(text)))
                {
                    return Color.FromHex("#b50000");
                }

                return Color.FromHex("#909090");
            }
        }

        KeyValuePair<string, string>? GetDisplayStatus()
        {
            KeyValuePair<string, string> status = LendingList.Where(s => s.Key == LibKey).FirstOrDefault();
            var internalStatus = status.Key == null ? -1 : GetStatusInternalNumber(status.Value);

            foreach (var pair in LendingList)
            {
                if (status.Key == null || internalStatus < GetStatusInternalNumber(pair.Value))
                {
                    status = pair;
                    internalStatus = GetStatusInternalNumber(pair.Value);
                }
            }

            if (status.Key == null) return null;

            return status;
        }

        int GetStatusInternalNumber(string status)
        {
            switch (status)
            {
                case "貸出可":
                case "蔵書あり":
                case "館内のみ":
                    return 20;
                case "貸出中":
                case "予約中":
                    return 10;
                case "準備中":
                case "休館中":
                case "蔵書なし":
                default:
                    return 0;
            }
        }
    }
}
