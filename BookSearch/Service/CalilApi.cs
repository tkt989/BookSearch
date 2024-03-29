﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using BookSearch.Model;

namespace BookSearch.Service
                    
{

    public class CalilApi
    {

        static readonly int DELAY = 200;
        static readonly string baseUrl = "https://api.calil.jp/";

        HttpClient client = new HttpClient();

        public CalilApi()
        {
            client.Timeout = TimeSpan.FromSeconds(10);
        }

        /// <summary>
        /// 図書館を検索する
        /// </summary>
        /// <returns>The library async.</returns>
        /// <param name="pref">Preference.</param>
        public async Task<List<LibraryEntity>> GetLibraryAsync(string pref, string city = null)
        {
            var url = baseUrl + $"library?format=json&callback=&pref={pref}&city={city}";
            var content = await client.GetStringAsync(url);
            var result = JsonConvert.DeserializeObject<List<LibraryEntity>>(content);
            return result;
        }

        /// <summary>
        /// 現在位置から図書館を検索
        /// </summary>
        /// <returns>The library async.</returns>
        /// <param name="longitude">Longitude.</param>
        /// <param name="latitude">Latitude.</param>
        public async Task<List<LibraryEntity>> GetLibraryAsync(double longitude, double latitude)
        {
            var url = baseUrl + $"library?format=json&callback=&limit=10&geocode={longitude},{latitude}";
            var content = await client.GetStringAsync(url);
            var result = JsonConvert.DeserializeObject<List<LibraryEntity>>(content);
            return result.OrderBy(library => library.Distance).ToList();
        }

        /// <summary>
        /// カーリルAPIから蔵書を探す
        /// </summary>
        /// <returns>The search.</returns>
        /// <param name="isbn">Isbn.</param>
        /// <param name="systemIdList">System identifier list.</param>
        /// <param name="callback">Callback.</param>
        public async Task Search(string isbn, List<string> systemIdList, Action<List<SearchResponse>> callback)
        {
            await SearchInternal(isbn, systemIdList, callback);
        }

        /// <summary>
        /// カーリルAPIから蔵書を探す
        /// </summary>
        /// <returns>The internal.</returns>
        /// <param name="isbn">Isbn.</param>
        /// <param name="systemIdList">System identifier list.</param>
        /// <param name="callback">Callback.</param>
        private async Task SearchInternal(string isbn, List<string> systemIdList, Action<List<SearchResponse>> callback)
        {
            await SearchInternalSessionAsync(isbn, systemIdList, null, callback);
        }

        /// <summary>
        /// カーリルAPIから蔵書を探す。With Session
        /// </summary>
        /// <returns>The internal session async.</returns>
        /// <param name="isbn">Isbn.</param>
        /// <param name="systemIdList">System identifier list.</param>
        /// <param name="session">Session.</param>
        /// <param name="callback">Callback.</param>
        private async Task SearchInternalSessionAsync(string isbn, List<string> systemIdList, string session, Action<List<SearchResponse>> callback)
        {
            var url = baseUrl + String.Format("check?format=json&callback=no&isbn={0}&systemid={1}",
                                              isbn, string.Join(",", systemIdList));
            if (session != null)
            {
                url = baseUrl + String.Format("check?format=json&callback=no&session={0}", session);
            }
            var content = client.GetStringAsync(url).Result;
            var json = JObject.Parse(content);
            var isContinue = json["continue"].Value<string>() == "1";

            var result = GetLibraryResult(json, isbn);

            callback.Invoke(result);

            if (isContinue)
            {
                await Task.Delay(2000);
                await SearchInternalSessionAsync(isbn, systemIdList, session, callback);
                return;
            }
        }

        /// <summary>
        /// カーリルAPIから蔵書を探す
        /// </summary>
        /// <returns>The search.</returns>
        /// <param name="isbn">Isbn.</param>
        /// <param name="systemIdList">System identifier list.</param>
        /// <param name="progress">Progress.</param>
        public async Task Search(string isbn, List<string> systemIdList, IProgress<List<SearchResponse>> progress = null)
        {
            await SearchWithSession(isbn, systemIdList, null, progress);
        }

        /// <summary>
        /// カーリルAPIから蔵書を探す
        /// </summary>
        /// <returns>The with session.</returns>
        /// <param name="isbn">Isbn.</param>
        /// <param name="systemIdList">System identifier list.</param>
        /// <param name="session">Session.</param>
        /// <param name="progress">Progress.</param>
        async Task SearchWithSession(string isbn, List<string> systemIdList, string session = null, IProgress<List<SearchResponse>> progress = null)
        {
            bool isContinue = true;

            while (isContinue)
            {
                var url = baseUrl +
                    $"check?format=json&callback=no&isbn={isbn}&systemid={string.Join(",", systemIdList)}&session={session}";
                var content = await client.GetStringAsync(url);
                var json = JObject.Parse(content);
                var result = GetLibraryResult(json, isbn);

                progress?.Report(result);

                isContinue = json["continue"].Value<string>() == "1";

                await Task.Delay(DELAY);
            }
        }

        /// <summary>
        /// jsonから図書館の蔵書情報を取得する
        /// </summary>
        /// <returns>The library result.</returns>
        /// <param name="json">Json.</param>
        /// <param name="isbn">Isbn.</param>
        List<SearchResponse> GetLibraryResult(JObject json, string isbn)
        {
            var result = new List<SearchResponse>();
            JObject libraryObj = json?["books"]?[isbn] as JObject;

            if (libraryObj == null)
            {
                return result;
            }

            foreach (var systemId in libraryObj.Properties())
            {
                var lib = libraryObj[systemId.Name];
                var libraryResult = lib.ToObject<SearchResponse>();
                libraryResult.SystemId = systemId.Name;
                result.Add(libraryResult);
            }

            return result;
        }
    }
}
