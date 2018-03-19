using System;
using System.Threading.Tasks;
using System.Net.Http;
using BookSearch.Model;
using Newtonsoft.Json.Linq;

namespace BookSearch.Service
{
    public class RakutenBookApi : IBookInfoApi
    {
        static readonly string APPID = "1001908046875407219";
        static readonly string BASEURL = $"https://app.rakuten.co.jp/services/api/BooksBook/Search/20170404?applicationId={APPID}";

        HttpClient client = new HttpClient();

        public async Task<BookInfo> GetBookInfoAsync(string isbn)
        {
            var url = BASEURL + $"&isbn={isbn}";
            var content = await client.GetStringAsync(url);

            var json = JObject.Parse(content);
            var items = json["Items"] as JArray;
            if (items == null || items.Count == 0) return null;

            var title = items[0]["Item"]["title"].Value<string>();
            var thumbnail = items[0]["Item"]["largeImageUrl"].Value<string>();

            return new BookInfo()
            {
                Title = title,
                Thumbnail = thumbnail,
                ISBN = isbn
            };
        }
    }
}
