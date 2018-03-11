using System;
using System.Linq;
using Realms;

namespace BookSearch.Model
{
    public class FavoriteLibraryEntity : RealmObject
    {
        static Realm realm;

        public string LibKey { get; set; }
        public string SystemId { get; set; }
        public string Name { get; set; }

        public static void Init(Realm realm)
        {
            FavoriteLibraryEntity.realm = realm;
        }

        public static IQueryable<FavoriteLibraryEntity> GetAll()
        {
            return realm.All<FavoriteLibraryEntity>();
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
