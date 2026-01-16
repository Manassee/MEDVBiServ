using MEDVBiServ.Contracts.Dtos;
using MEDVBiServ.Contracts.Enums;


namespace MEDVBiServ.Interfaces
{
    public interface IBibleApiClient
    {
        Task<List<BookInfos>> GetBooksAsync(LanguageCode translation);
        Task<List<int>> GetChaptersAsync(LanguageCode translation, int bookNumber);
        Task<List<BibleVerseDto>> GetVersesFromChapterAsync(LanguageCode translation, int bookNumber, int chapter);
        Task<BibleVerseDto?> GetSingleVerseAsync(LanguageCode translation, int bookNumber, int chapter, int verseNumber);
        Task<List<BibleVerseDto>> GetVerseRangeAsync(LanguageCode translation, int bookNumber, int chapter, int fromVerse, int toVerse);
    }
}
