using MEDVBiServ.Contracts.Dtos;
using MEDVBiServ.Contracts.Enums;
using MEDVBiServ.Interfaces;
using System.Net;
using System.Net.Http.Json;

namespace MEDVBiServ.Clients
{
    public sealed class BibleApiClient : IBibleApiClient
    {

        private readonly HttpClient _http;
        public BibleApiClient(HttpClient http) => _http = http;


        public async Task<List<BookInfos>> GetBooksAsync(LanguageCode translation)
            => await _http.GetFromJsonAsync<List<BookInfos>>(
                $"api/bible/books?translation={translation}"
            ) ?? new();

        public async Task<List<int>> GetChaptersAsync(LanguageCode translation, int bookNumber)
            => await _http.GetFromJsonAsync<List<int>>($"api/bible/books/{bookNumber}/chapters?translation={translation}") ?? new();

        public async Task<List<BibleVerseDto>> GetVersesFromChapterAsync(LanguageCode translation, int bookNumber, int chapter)
            => await _http.GetFromJsonAsync<List<BibleVerseDto>>(
                $"api/bible/books/{bookNumber}/chapters/{chapter}/verses?translation={translation}") ?? new();

        public async Task<BibleVerseDto?> GetSingleVerseAsync(LanguageCode translation, int bookNumber, int chapter, int verseNumber)
        {
            var url = $"api/bible/books/{bookNumber}/chapters/{chapter}/verses/{verseNumber}?translation={translation}";
            var res = await _http.GetAsync(url);
            if (res.StatusCode == HttpStatusCode.NotFound) return null;
            res.EnsureSuccessStatusCode();
            return await res.Content.ReadFromJsonAsync<BibleVerseDto>();
        }


        public Task<List<BibleVerseDto>> GetVerseRangeAsync(LanguageCode translation, int bookNumber, int chapter, int fromVerse, int toVerse)
        {
            throw new NotImplementedException();
        }

        
    }
}
