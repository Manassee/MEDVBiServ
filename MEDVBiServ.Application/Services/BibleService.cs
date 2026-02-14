using MEDVBiServ.Application.Dtos;
using MEDVBiServ.Application.Enums;
using MEDVBiServ.Application.Interfaces;
using MEDVBiServ.Application.Mapper;
using MEDVBiServ.Contracts.Dtos;
using MEDVBiServ.Contracts.Enums;
using MEDVBiServ.Contracts.Requests;

namespace MEDVBiServ.Application.Services
{
    public class BibleService : IBibleService
    {
        private readonly IBibleVerseRepository _repository;
        private readonly ILanguageProvider _languageProvider;

        public BibleService(IBibleVerseRepository repository, ILanguageProvider languageProvider)
        {
            _repository = repository;
            _languageProvider = languageProvider;
        }

        private static Translation MapToTranslation(LanguageCode lang) => lang switch
        {
            LanguageCode.De => Translation.De,
            LanguageCode.Fr => Translation.Fr,
            _ => Translation.De
        };


        public async Task<IReadOnlyList<Dtos.BookInfos>> GetBooksAsync(Translation translation)
        {
            var languageCode = _languageProvider.GetLanguageCode(translation); // "de" / "fr"
            var bookNumbers = await _repository.GetAllBooksAsync(languageCode);

            return bookNumbers
                .Select(bookNumber => new Dtos.BookInfos
                {
                    Id = bookNumber,
                    Name = _languageProvider.GetBookName(translation, bookNumber),
                })
                .ToList();
        }

        public async Task<IReadOnlyList<int>> GetChaptersAsync(Translation translation, int bookNumber)
        {
            var languageCode = _languageProvider.GetLanguageCode(translation);
            return await _repository.GetChaptersAsync(bookNumber, languageCode);
        }

        public async Task<IReadOnlyList<VerseDto>> GetVersesFromChapterAsync(
            Translation translation, int bookNumber, int chapter)
        {
            var languageCode = _languageProvider.GetLanguageCode(translation);
            var entities = await _repository.GetVersesFromChapterAsync(bookNumber, chapter, languageCode);

            var bookName = _languageProvider.GetBookName(translation, bookNumber);

            return entities
                .Select(v => BibleVerseMapper.ToDto(v, bookName))
                .ToList();
        }

        public async Task<VerseDto?> GetSingleVerseAsync(
            Translation translation, int bookNumber, int chapter, int verseNumber)
        {
            var languageCode = _languageProvider.GetLanguageCode(translation);
            var entity = await _repository.GetSingleVerseAsync(bookNumber, chapter, verseNumber, languageCode);

            if (entity is null) return null;

            var bookName = _languageProvider.GetBookName(translation, bookNumber);
            return BibleVerseMapper.ToDto(entity, bookName);
        }

        public async Task<IReadOnlyList<VerseDto>> GetVerseRangeAsync(
            Translation translation, int bookNumber, int chapter, int fromVerse, int toVerse)
        {
            var languageCode = _languageProvider.GetLanguageCode(translation);
            var entities = await _repository.GetVerseRangeAsync(bookNumber, chapter, fromVerse, toVerse, languageCode);

            var bookName = _languageProvider.GetBookName(translation, bookNumber);

            return entities
                .Select(v => BibleVerseMapper.ToDto(v, bookName))
                .ToList();
        }

        public async Task<VerseDto?> GetVerseByIdAsync(Translation translation, int id)
        {
            var languageCode = _languageProvider.GetLanguageCode(translation);
            var entity = await _repository.GetVersById(id, languageCode);

            if (entity is null) return null;

            var bookName = _languageProvider.GetBookName(translation, entity.Book);
            return BibleVerseMapper.ToDto(entity, bookName);
        }

        

        public async Task<PagedResultDto<VerseDto>> GetVersesPagedAsync(
    GetBibleVersesRequest request,
    CancellationToken ct = default)
        {
            var page = request.Page < 1 ? 1 : request.Page;
            var pageSize = request.PageSize < 1 ? 1 : request.PageSize;

            const int MaxPageSize = 200;
            if (pageSize > MaxPageSize) pageSize = MaxPageSize;

            // ✅ Contracts.LanguageCode -> Application.Translation
            var translation = MapToTranslation(request.Language);

            // ✅ jetzt passt es zum ILanguageProvider
            var lang = _languageProvider.GetLanguageCode(translation);

            var sortBy = request.SortBy switch
            {
                VerseSortBy.Id => "Id",
                VerseSortBy.Book => "Book",
                VerseSortBy.Chapter => "Chapter",
                VerseSortBy.Verse => "Verse",
                _ => "Book"
            };

            var total = await _repository.CountAsync(lang, request.BookNumber, request.Chapter, request.Search, ct);

            var entities = total == 0
                ? []
                : await _repository.GetPagedAsync(
                    lang,
                    request.BookNumber,
                    request.Chapter,
                    request.Search,
                    page,
                    pageSize,
                    sortBy,
                    request.Desc,
                    ct);

            var items = entities.Select(v =>
            {
                var bookName = _languageProvider.GetBookName(translation, v.Book);
                return BibleVerseMapper.ToDto(v, bookName);
            }).ToList();

            return new PagedResultDto<VerseDto>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = total
            };
        }

    }
}
