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
            await Search(isbn, LibraryEntity.GetAll().ToList(), callback);
        }

        public async Task Search(string isbn, List<LibraryEntity> libraries, Action<List<SearchResult>> callback)
        {
            var systemIdList = libraries.Select(library => library.SystemId).Distinct().ToList();

            await calilApi.Search(isbn, systemIdList, response =>
            {
                var result = new List<SearchResult>();

                foreach (var library in libraries)
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

        public async Task SearchInLocation(string isbn, double longitude, double latitude, Action<List<SearchResult>> callback)
        {
            var nearbyLibraries = await calilApi.GetLibraryAsync(longitude, latitude);
            await Search(isbn, nearbyLibraries, callback);
        }
    }
}
