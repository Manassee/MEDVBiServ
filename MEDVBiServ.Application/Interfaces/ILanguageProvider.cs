using MEDVBiServ.Application.Dtos;
using MEDVBiServ.Application.Enums;


namespace MEDVBiServ.Application.Interfaces
{
    public interface ILanguageProvider
    {
        Translation Resolve(string? routeLang, string? queryLang, string? headerLang);
        // 2. Hole den 2-Letter Language Code ("de" / "fr")
        // 2-Letter-Code für DB-Auswahl ("de", "fr")
        string GetLanguageCode(Translation translation);

        // Name für ein Buch (nutzt ElberfelderMap / LouisSegondMap)
        string GetBookName(Translation translation, int bookNumber);

        // Alle Bücher mit Infos (DTO) für UI/API
        IReadOnlyList<BookInfos> GetAllBooks(Translation translation);
    }

}
