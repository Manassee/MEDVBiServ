using MEDVBiServ.Application.Dtos;
using MEDVBiServ.Application.Enums;
using MEDVBiServ.Application.Interfaces;
using MEDVBiServ.Application.Mapper;
using static MEDVBiServ.Application.Mapper.ElberfelderMap;

namespace MEDVBiServ.Application.Services
{
    public class LanguageProvider : ILanguageProvider
    {
        public Translation Resolve(string? routeLang, string? queryLang, string? headerLang)
        {
            var lang = routeLang ?? queryLang ?? headerLang;

            if (string.IsNullOrWhiteSpace(lang))
                return Translation.De;

            lang = lang.Trim().ToLowerInvariant();

            return lang switch
            {
                "de" or "ger" or "german" => Translation.De,
                "fr" or "fra" or "fre" or "french" => Translation.Fr,
                _ => Translation.De
            };
        }

        public string GetLanguageCode(Translation translation)
            => translation switch
            {
                Translation.De => "de",
                Translation.Fr => "fr",
                _ => "de"
            };

        public string GetBookName(Translation translation, int bookNumber)
            => translation switch
            {
                Translation.De => ElberfelderMap.GetName(bookNumber),
                Translation.Fr => LouisSegondMap.GetName(bookNumber),
                _ => ElberfelderMap.GetName(bookNumber)
            };

        public IReadOnlyList<BookInfos> GetAllBooks(Translation translation)
        {
            // ElberfelderMap & LouisSegondMap liefern beide BookDef[]
            IReadOnlyList<BookDef> src = translation switch
            {
                Translation.De => ElberfelderMap.GetAll(),
                Translation.Fr => LouisSegondMap.GetAll(),
                _ => ElberfelderMap.GetAll()
            };

            var list = src
                .Select(b => new BookInfos
                {
                    Id = b.Id,
                    Code = b.Code,
                    Name = b.Name,
                    Testament = b.T, // Korrigiert: Testament ist vom Typ Testament, nicht string
                    // Chapters ggf. ergänzen, falls benötigt
                })
                .ToList();

            return list;
        }
    }
}
