using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookSearch.Model;

namespace BookSearch.Service
{
    public class LibraryRepository
    {
        CalilApi calilApi = new CalilApi();
        
        public async Task<List<LibraryEntity>> Search(string pref, string city = null)
        {
            var entities = await calilApi.GetLibraryAsync(pref, city);
            return entities;
        }

        public async Task<List<LibraryEntity>> Search(double longitude, double latitude)
        {
            var entities = await calilApi.GetLibraryAsync(longitude, latitude);
            return entities;
        }
    }
}
