using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BookSearch.Model
{
    public class SearchResponse
    {
        public string SystemId { get; set; }
        
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("reserveUrl")]
        public string ReserveUrl { get; set; }

        [JsonProperty("libkey")]
        public Dictionary<string, string> LibKeyMap { get; set; } = new Dictionary<string, string>();

        public override string ToString()
        {
            var libkey = string.Join(", ", LibKeyMap.Select((key) =>
            {
                return $"{key.Key}: {key.Value}";
            }));
            return string.Format("[LibraryResult: SystemId={0}, Status={1}, ReserveUrl={2}, Libkey={3}]", SystemId, Status, ReserveUrl, libkey);
        }
    }
}
