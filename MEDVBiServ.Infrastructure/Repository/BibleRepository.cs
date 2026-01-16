using MEDVBiServ.Domain.Entities;
using MEDVBiServ.Infrastructure.DbContext;
using MEDVBiServ.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDVBiServ.Infrastructure.Repository
{
    public class BibleRepository : IBibleVerseRepository
    {

        private readonly DEDbContext _deContext;
        private readonly FRDbContext _frContext;
        public BibleRepository(DEDbContext deDbContext, FRDbContext frDbContext)
        {
            _deContext = deDbContext;
            _frContext = frDbContext;
        }


        // Helper: Wähle je nach Sprache den richtigen DbSet<Bible>
        private DbSet<Bible> GetDbSet(string language)
        {
            if (string.Equals(language, "de", StringComparison.OrdinalIgnoreCase))
                return _deContext.Verses;   // Name des DbSet anpassen!

            if (string.Equals(language, "fr", StringComparison.OrdinalIgnoreCase))
                return _frContext.Verses;   // Name des DbSet anpassen!

            throw new ArgumentException($"Nicht unterstütze Sprache'{language}'. Benutze 'de' oder 'fr'.", nameof(language));
        }

        private IQueryable<Bible> Query(string language) => GetDbSet(language).AsQueryable();

        public async Task<List<int>> GetAllBooksAsync(string language)
        {
            return await Query(language)
                .Select(v => v.Book)
                .Distinct()
                .OrderBy(b => b)
                .ToListAsync();
        }

        public async Task<List<Bible>> GetAllVersAsync(string language)
        {
            return await Query(language)
                .OrderBy(v => v.Book)
                .ThenBy(v => v.Chapter)
                .ThenBy(v => v.Verse)   
                .ToListAsync();
        }

        public async Task<List<int>> GetChaptersAsync(int bookNumber, string language)
        {
            return await Query(language)
                .Where(v => v.Book == bookNumber)
                .Select(v => v.Chapter)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();
        }

        public async Task<Bible?> GetSingleVerseAsync(int bookNumber, int chapter, int verseNumber, string language)
        {
            return await Query(language)
                .FirstOrDefaultAsync(v =>
                    v.Book == bookNumber &&
                    v.Chapter == chapter &&
                    v.Verse == verseNumber);
        }

        public async Task<Bible?> GetVersById(int id, string language)
        {
            return await Query(language)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<List<Bible>> GetVerseRangeAsync(int bookNumber, int chapter, int fromVerse, int toVerse, string language)
        {
            return await Query(language)
                .Where(v =>
                    v.Book == bookNumber &&
                    v.Chapter == chapter &&
                    v.Verse >= fromVerse &&
                    v.Verse <= toVerse)
                .OrderBy(v => v.Verse)
                .ToListAsync();
        }

        public async Task<List<Bible>> GetVersesFromChapterAsync(int bookNumber, int chapter, string language)
        {
            return await Query(language)
                .Where(v => v.Book == bookNumber && v.Chapter == chapter)
                .OrderBy(v => v.Verse)
                .ToListAsync();
        }

        public async Task<int> CountAsync(string language, CancellationToken ct = default)
        {
            return await GetDbSet(language).CountAsync(ct);
        }

        public async Task<List<Bible>> GetPagedAsync(string language, int page, int pageSize, CancellationToken ct = default)
        {
            var skip = (page - 1) * pageSize;

            return await GetDbSet(language)
                .AsNoTracking()
                .OrderBy(v => v.Id)               
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync(ct);
        }

        public async Task<int> CountVersesInChapterAsync(int bookNumber, int chapter, string languageCode, CancellationToken ct = default)
        {
            return await GetDbSet(languageCode)
                .AsNoTracking()
                .CountAsync(v => v.Book == bookNumber && v.Chapter == chapter, ct);
        }

        // ✅ NEU: DB-seitiges Paging
        public async Task<IReadOnlyList<Bible>> GetVersesFromChapterPagedAsync(
            int bookNumber, int chapter, string languageCode, int page, int pageSize, CancellationToken ct = default)
        {
            var skip = (page - 1) * pageSize;

            return await GetDbSet(languageCode)
                .AsNoTracking()
                .Where(v => v.Book == bookNumber && v.Chapter == chapter)
                .OrderBy(v => v.Verse) // wichtige stabile Sortierung!
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync(ct);
        }

        

        private static IQueryable<Bible> ApplyFilters(
            IQueryable<Bible> q,
            int? bookNumber,
            int? chapter,
            string? search)
        {
            if (bookNumber.HasValue)
                q = q.Where(v => v.Book == bookNumber.Value);

            if (chapter.HasValue)
                q = q.Where(v => v.Chapter == chapter.Value);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                q = q.Where(v => v.Text.Contains(s));
                // SQLite: Contains -> LIKE, ok. (Case-Sensitivity hängt von Collation ab)
            }

            return q;
        }

        private static IQueryable<Bible> ApplySorting(IQueryable<Bible> q, string sortBy, bool desc)
        {
            // stabile Sortierung: immer Secondary Keys nutzen, damit Paging “stabil” bleibt
            return (sortBy, desc) switch
            {
                ("Id", true) => q.OrderByDescending(v => v.Id),
                ("Id", false) => q.OrderBy(v => v.Id),

                ("Book", true) => q.OrderByDescending(v => v.Book).ThenByDescending(v => v.Chapter).ThenByDescending(v => v.Verse),
                ("Book", false) => q.OrderBy(v => v.Book).ThenBy(v => v.Chapter).ThenBy(v => v.Verse),

                ("Chapter", true) => q.OrderByDescending(v => v.Chapter).ThenByDescending(v => v.Verse).ThenByDescending(v => v.Id),
                ("Chapter", false) => q.OrderBy(v => v.Chapter).ThenBy(v => v.Verse).ThenBy(v => v.Id),

                ("Verse", true) => q.OrderByDescending(v => v.Verse).ThenByDescending(v => v.Id),
                ("Verse", false) => q.OrderBy(v => v.Verse).ThenBy(v => v.Id),

                _ => q.OrderBy(v => v.Book).ThenBy(v => v.Chapter).ThenBy(v => v.Verse) // Default
            };
        }

        public async Task<int> CountAsync(string languageCode, int? bookNumber, int? chapter, string? search, CancellationToken ct = default)
        {
            var q = Query(languageCode);
            q = ApplyFilters(q, bookNumber, chapter, search);
            return await q.CountAsync(ct);
        }

        public async Task<IReadOnlyList<Bible>> GetPagedAsync(string languageCode, int? bookNumber, int? chapter, string? search, int page, int pageSize, string sortBy, bool desc, CancellationToken ct = default)
        {
            var skip = (page - 1) * pageSize;

            var q = Query(languageCode);
            q = ApplyFilters(q, bookNumber, chapter, search);
            q = ApplySorting(q, sortBy, desc);

            return await q.Skip(skip).Take(pageSize).ToListAsync(ct);
        }
    }
}
