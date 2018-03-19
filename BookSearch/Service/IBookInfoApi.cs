using System;
using System.Threading.Tasks;
using BookSearch.Model;

namespace BookSearch.Service
{
    public interface IBookInfoApi
    {
        Task<BookInfo> GetBookInfoAsync(string isbn);
    }
}
