using System;
using System.Collections.Generic;
using System.Linq;
using Realms;
using Newtonsoft.Json;

namespace BookSearch.Model
{
    public class LibraryEntity : RealmObject
    {
        static Realm realm;

        [JsonProperty("systemid")]
        public string SystemId { get; set; }

        [JsonProperty("systemname")]
        public string SystemName { get; set; }

        [JsonProperty("libid")]
        public string LibId { get; set; }

        [JsonProperty("libkey")]
        public string LibKey { get; set; }

        [JsonProperty("formal")]
        public string Name { get; set; }

        [JsonProperty("pref")]
        public string Pref { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("distance")]
        public double Distance { get; set; }

        [JsonProperty("url_pc")]
        public string URL { get; set; }

        [JsonProperty("geocode")]
        public string Geocode { get; set; }

        public bool IsFavorite { get; set; }

        public static void Init(Realm realm)
        {
            LibraryEntity.realm = realm;
        }

        public static IQueryable<LibraryEntity> GetAll()
        {
            return realm.All<LibraryEntity>();
        }

        public void Write()
        {
            realm.Write(() => {
                realm.Add(this, true);
            });
        }

        public void Remove()
        {
            realm.Write(() => {
                realm.Remove(this);
            });
        }
    }
}
