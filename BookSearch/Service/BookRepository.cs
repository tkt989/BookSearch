using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using BookSearch.Model;

namespace BookSearch.Service
{
    public class BookRepository
    {
        GoogleBookApi googleBookApi = new GoogleBookApi();
        CalilApi calilApi = new CalilApi();

        public async Task<BookInfo> SearchBookInfo(string isbn)
        {
            return await googleBookApi.GetBookInfoAsync(isbn);
        }

        public async Task SearchInFavorites(string isbn, Action<List<SearchResult>> callback)
        {
            var favorites = LibraryEntity.GetAll().ToList();
            var systemIdList = favorites.Select(library => library.SystemId).Distinct().ToList();

            await calilApi.Search(isbn, systemIdList, response =>
            {
                var result = new List<SearchResult>();

                foreach (var library in favorites)
                {
                    var match = response.Where(r => r.SystemId == library.SystemId).FirstOrDefault();
                    if (match != null)
                    {
                        result.Add(SearchResult.FromEntity(library, match));
                    }
                }

                callback?.Invoke(result);
            });
        }
    }
}
