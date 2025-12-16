using MEDVBiServ.Application.Dtos;
using MEDVBiServ.Application.Enums;
using MEDVBiServ.Application.Interfaces;
using MEDVBiServ.Application.Mapper;
using MEDVBiServ.Contracts.Dtos;
using MEDVBiServ.Infrastructure.Interfaces;

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

        public async Task<IReadOnlyList<BookInfos>> GetBooksAsync(Translation translation)
        {
            var languageCode = _languageProvider.GetLanguageCode(translation); // "de" / "fr"
            var bookNumbers = await _repository.GetAllBooksAsync(languageCode);

            return bookNumbers
                .Select(bookNumber => new BookInfos
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

        // ✅ Paging-Usecase (DB-seitig über Repository)
        public async Task<PagedResultDto<VerseDto>> GetVersesPagedAsync(
            Translation translation, int bookNumber, int chapter, int page, int pageSize,
            CancellationToken ct = default)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 1;

            const int MaxPageSize = 200;
            if (pageSize > MaxPageSize) pageSize = MaxPageSize;

            var languageCode = _languageProvider.GetLanguageCode(translation);

            var totalCount = await _repository.CountVersesInChapterAsync(bookNumber, chapter, languageCode, ct);

            if (totalCount == 0)
            {
                return new PagedResultDto<VerseDto>
                {
                    Items = [],
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = 0
                };
            }

            var entities = await _repository.GetVersesFromChapterPagedAsync(
                bookNumber, chapter, languageCode, page, pageSize, ct);

            var bookName = _languageProvider.GetBookName(translation, bookNumber);

            var items = entities
                .Select(v => BibleVerseMapper.ToDto(v, bookName))
                .ToList();

            return new PagedResultDto<VerseDto>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }
    }
}
