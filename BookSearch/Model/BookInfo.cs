using System;
using System.Linq;
using System.Threading.Tasks;
using Realms;
using Xamarin.Forms;

namespace BookSearch.Model
{
    public class BookInfo : RealmObject
    {
        public static Realm Realm { get; set; }

        public string Title { get; set; }
        public string ISBN { get; set; }
        public string Thumbnail
        {
            get; set;
        }

        public static IQueryable<BookInfo> GetAll()
        {
            return Realm.All<BookInfo>();
        }

        public void Write()
        {
            Realm.Write(() =>
            {
                Realm.Add(this, true);
            });
        }

        public void Remove()
        {
            Realm.Write(() =>
            {
                Realm.Remove(this);
            });
        }

		public override bool Equals(object obj)
		{
            if (obj is BookInfo info)
            {
                return this.ISBN == info.ISBN;
            }
            return false;
		}
	}
}
