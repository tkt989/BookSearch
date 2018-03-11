using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using BookSearch.Model;

namespace BookSearch.Service
{
    public class GoogleBookApi
    {
        private const string infoUrl = "https://www.googleapis.com/books/v1/volumes?q=isbn:{0}";

        private HttpClient client = new HttpClient();

        public async Task<BookInfo> GetBookInfoAsync(string isbn)
        {
            var url = String.Format(infoUrl, isbn);
            var content = await client.GetStringAsync(url);

            var json = JObject.Parse(content);
            var totalItem = json["totalItems"].Value<int>();

            if (totalItem == 0) return null;

            var obj = json["items"][0]["volumeInfo"];

            var info = new BookInfo();
            info.Title = obj["title"].Value<string>();
            info.Thumbnail = obj["imageLinks"]["thumbnail"].Value<string>();

            return info;
        }
    }
}