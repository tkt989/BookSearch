using System;
using System.Linq;
using System.Threading.Tasks;
using Realms;
using Xamarin.Forms;

namespace BookSearch.Model
{
    public class BookInfo : RealmObject
    {
        static readonly int SIZE = 20;

        public static Realm Realm { get; set; }

        public string Title { get; set; }
        public string ISBN { get; set; }
        public string Thumbnail { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public static IQueryable<BookInfo> GetAll()
        {
            return Realm.All<BookInfo>().OrderByDescending(info => info.CreatedAt);
        }

        public void Write()
        {
            Realm.Write(() =>
            {
                this.CreatedAt = DateTimeOffset.Now;
                Realm.Add(this, true);
            });

            Clean();
        }

        void Remove()
        {
            Realm.Write(() =>
            {
                Realm.Remove(this);
            });
        }

        void Clean()
        {
            while (GetAll().Count() > SIZE)
            {
                var info = GetAll().OrderByDescending(entity => entity.CreatedAt).FirstOrDefault();
                if (info != null) info.Remove();
            }
        }

		public override bool Equals(object obj)
		{
            if (obj is BookInfo info)
            {
                return this.ISBN == info.ISBN;
            }
            return false;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
