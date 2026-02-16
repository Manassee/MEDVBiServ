using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Encodings.Web;
using MEDVBiServ.Contracts.Dtos;
using MEDVBiServ.Contracts.Enums;
using MEDVBiServ.Interfaces;

namespace MEDVBiServ.Clients;

public sealed class BibleApiClient : IBibleApiClient
{
    private readonly HttpClient _http;

    public BibleApiClient(HttpClient http)
    {
        _http = http;
        Console.WriteLine($"BibleApiClient BaseAddress: {_http.BaseAddress}");
    }

    public Task<List<BookInfos>> GetBooksAsync(LanguageCode translation)
        => GetOrThrowAsync<List<BookInfos>>(
            $"api/bible/books?translation={UrlEncode(translation)}");

    public Task<List<int>> GetChaptersAsync(LanguageCode translation, int bookNumber)
        => GetOrThrowAsync<List<int>>(
            $"api/bible/books/{bookNumber}/chapters?translation={UrlEncode(translation)}");

    public Task<List<BibleVerseDto>> GetVersesFromChapterAsync(LanguageCode translation, int bookNumber, int chapter)
        => GetOrThrowAsync<List<BibleVerseDto>>(
            $"api/bible/books/{bookNumber}/chapters/{chapter}/verses?translation={UrlEncode(translation)}");

    public async Task<BibleVerseDto?> GetSingleVerseAsync(LanguageCode translation, int bookNumber, int chapter, int verseNumber)
    {
        var url =
            $"api/bible/books/{bookNumber}/chapters/{chapter}/verses/{verseNumber}?translation={UrlEncode(translation)}";

        using var res = await _http.GetAsync(url);

        if (res.StatusCode == HttpStatusCode.NotFound)
            return null;

        await EnsureSuccessWithBodyAsync(res, url);

        return await res.Content.ReadFromJsonAsync<BibleVerseDto>();
    }

    public Task<List<BibleVerseDto>> GetVerseRangeAsync(LanguageCode translation, int bookNumber, int chapter, int fromVerse, int toVerse)
        => GetOrThrowAsync<List<BibleVerseDto>>(
            $"api/bible/books/{bookNumber}/chapters/{chapter}/verses/range" +
            $"?fromVerse={fromVerse}&toVerse={toVerse}&translation={UrlEncode(translation)}");

    // -----------------------------
    // Helpers
    // -----------------------------

    private async Task<T> GetOrThrowAsync<T>(string url)
    {
        using var res = await _http.GetAsync(url);
        await EnsureSuccessWithBodyAsync(res, url);

        // Falls API mal "null" liefert: robust bleiben
        var data = await res.Content.ReadFromJsonAsync<T>();
        return data ?? CreateEmpty<T>();
    }

    private static async Task EnsureSuccessWithBodyAsync(HttpResponseMessage res, string url)
    {
        if (res.IsSuccessStatusCode) return;

        var body = res.Content is null ? "" : await res.Content.ReadAsStringAsync();

        throw new HttpRequestException(
            $"API-Fehler {(int)res.StatusCode} ({res.ReasonPhrase}) bei '{url}'. Body: {body}");
    }

    private static string UrlEncode(LanguageCode translation)
        => WebUtility.UrlEncode(translation.ToString());

    private static T CreateEmpty<T>()
    {
        // Für die typischen Listen, die du zurückgibst
        if (typeof(T) == typeof(List<BookInfos>)) return (T)(object)new List<BookInfos>();
        if (typeof(T) == typeof(List<int>)) return (T)(object)new List<int>();
        if (typeof(T) == typeof(List<BibleVerseDto>)) return (T)(object)new List<BibleVerseDto>();

        return default!;
    }
}
