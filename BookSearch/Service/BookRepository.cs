using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using BookSearch.Model;

namespace BookSearch.Service
{
    public class BookRepository
    {
        //IBookInfoApi bookInfoApi = new GoogleBookApi();
        List<IBookInfoApi> bookApis = new List<IBookInfoApi>()
        {
            new RakutenBookApi(),
            new GoogleBookApi()
        };
        CalilApi calilApi = new CalilApi();

        public async Task<BookInfo> SearchBookInfo(string isbn)
        {
            var tasks = bookApis.Select(api => api.GetBookInfoAsync(isbn)).ToList();

            while (tasks.Count() != 0)
            {
                var task = await Task.WhenAny(tasks);
                var info = task.Result;
                if (info == null)
                {
                    tasks.Remove(task);
                    continue;
                }

                return info;
            }
            return null;
        }

        public async Task SearchInFavorites(string isbn, Action<List<SearchResult>> callback)
        {
            await Search(isbn, LibraryEntity.GetAll().ToList(), callback);
        }

        public async Task SearchInFavorites(string isbn, IProgress<List<SearchResult>> progress = null)
        {
            await Search(isbn, LibraryEntity.GetAll().ToList(), progress);
        }

        public async Task Search(string isbn, List<LibraryEntity> libraries, Action<List<SearchResult>> callback)
        {
            callback?.Invoke(libraries.Select(library => SearchResult.FromEntity(library)).ToList());

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

        public async Task Search(string isbn, List<LibraryEntity> libraries, IProgress<List<SearchResult>> progress)
        {
            progress?.Report(libraries.Select(library => SearchResult.FromEntity(library)).ToList());

            var systemIdList = libraries.Select(library => library.SystemId).Distinct().ToList();

            await calilApi.Search(isbn, systemIdList, new Progress<List<SearchResponse>>(responseList =>
            {
                var result = new List<SearchResult>();

                foreach (var library in libraries)
                {
                    var matchResponse = responseList.Where(response => response.SystemId == library.SystemId).FirstOrDefault();
                    if (matchResponse != null)
                    {
                        result.Add(SearchResult.FromEntity(library, matchResponse));
                    }
                }

                progress?.Report(result);
            }));
        }

        public async Task SearchInLocation(string isbn, double longitude, double latitude, Action<List<SearchResult>> callback)
        {
            var nearbyLibraries = await calilApi.GetLibraryAsync(longitude, latitude);
            await Search(isbn, nearbyLibraries, callback);
        }

        public async Task SearchInLocation(string isbn, double longitude, double latitude, IProgress<List<SearchResult>> progress)
        {
            var nearbyLibraries = await calilApi.GetLibraryAsync(longitude, latitude);
            await Search(isbn, nearbyLibraries, progress);
        }
    }
}
