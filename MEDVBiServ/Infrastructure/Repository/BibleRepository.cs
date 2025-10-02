// Infrastructure/Repository/BibleRepository{TContext}.cs
using MEDVBiServ.Application.Dtos;
using MEDVBiServ.Domain.Entities;
using MEDVBiServ.Infrastructure.Repository.interfaces;
using Microsoft.EntityFrameworkCore;

namespace MEDVBiServ.Infrastructure.Repository
{
    public class BibleRepository<TContext> : IBibleRepository
        where TContext : Microsoft.EntityFrameworkCore.DbContext
    {
        private readonly IDbContextFactory<TContext> _factory;

        public BibleRepository(IDbContextFactory<TContext> factory)
            => _factory = factory;

        private async Task<TResult> WithDb<TResult>(
            Func<TContext, Task<TResult>> action, CancellationToken ct)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await action(db);
        }

        // --- Bücher ---

        public Task<IReadOnlyList<BookInfos>> GetBooksAsync(CancellationToken ct = default)
            => WithDb(async db =>
            {
                var rows = await db.Set<Bible>()
                    .AsNoTracking()
                    .GroupBy(v => v.Book)
                    .Select(g => new { Book = g.Key, Chapters = g.Max(x => x.Chapter) })
                    .OrderBy(x => x.Book)
                    .ToListAsync(ct);

                // BookInfos = dein DTO-Typ (achte auf korrekten Namen/Namensraum)
                return (IReadOnlyList<BookInfos>)rows
                    .Select(x => new BookInfos
                    {
                        Id = x.Book,
                        Chapters = x.Chapters
                        // optional: Name/Code/Testament via Map ergänzen
                    })
                    .ToList();
            }, ct);

        public Task<BookInfos?> GetBookAsync(int book, CancellationToken ct = default)
            => WithDb(async db =>
            {
                var maxChapter = await db.Set<Bible>()
                    .AsNoTracking()
                    .Where(v => v.Book == book)
                    .Select(v => v.Chapter)
                    .DefaultIfEmpty()
                    .MaxAsync(ct);

                if (maxChapter == 0) return null;

                return new BookInfos
                {
                    Id = book,
                    Chapters = maxChapter
                    // optional: Name/Code/Testament via Map ergänzen
                };
            }, ct);

        public Task<IReadOnlyList<int>> GetChapterNumbersAsync(int book, CancellationToken ct = default)
            => WithDb(async db =>
                (IReadOnlyList<int>)await db.Set<Bible>()
                    .AsNoTracking()
                    .Where(v => v.Book == book)
                    .Select(v => v.Chapter)
                    .Distinct()
                    .OrderBy(c => c)
                    .ToListAsync(ct), ct);

        // --- Kapitel/Verse ---

        public Task<IReadOnlyList<VerseDto>> GetChapterAsync(int book, int chapter, CancellationToken ct = default)
            => WithDb(async db =>
                (IReadOnlyList<VerseDto>)await db.Set<Bible>()
                    .AsNoTracking()
                    .Where(v => v.Book == book && v.Chapter == chapter)
                    .OrderBy(v => v.Verse)
                    .Select(v => new VerseDto
                    {
                        Book = v.Book,
                        Chapter = v.Chapter,
                        Verse = v.Verse,
                        Text = v.Text
                    })
                    .ToListAsync(ct), ct);

        public Task<VerseDto?> GetVerseAsync(int book, int chapter, int verse, CancellationToken ct = default)
            => WithDb(async db =>
                await db.Set<Bible>()
                    .AsNoTracking()
                    .Where(v => v.Book == book && v.Chapter == chapter && v.Verse == verse)
                    .Select(v => new VerseDto
                    {
                        Book = v.Book,
                        Chapter = v.Chapter,
                        Verse = v.Verse,
                        Text = v.Text
                    })
                    .FirstOrDefaultAsync(ct), ct);

        public Task<IReadOnlyList<VerseDto>> GetRangeAsync(int book, int chapter, int fromVerse, int toVerse, CancellationToken ct = default)
            => WithDb(async db =>
                (IReadOnlyList<VerseDto>)await db.Set<Bible>()
                    .AsNoTracking()
                    .Where(v => v.Book == book && v.Chapter == chapter && v.Verse >= fromVerse && v.Verse <= toVerse)
                    .OrderBy(v => v.Verse)
                    .Select(v => new VerseDto
                    {
                        Book = v.Book,
                        Chapter = v.Chapter,
                        Verse = v.Verse,
                        Text = v.Text
                    })
                    .ToListAsync(ct), ct);

        // --- Suche ---

        public Task<IReadOnlyList<VerseDto>> SearchAsync(string term, int? book = null, int limit = 50, CancellationToken ct = default)
            => WithDb(async db =>
            {
                var q = db.Set<Bible>().AsNoTracking()
                    .Where(v => EF.Functions.Like(v.Text, $"%{term}%"));
                if (book.HasValue) q = q.Where(v => v.Book == book.Value);

                return (IReadOnlyList<VerseDto>)await q
                    .OrderBy(v => v.Book).ThenBy(v => v.Chapter).ThenBy(v => v.Verse)
                    .Take(limit)
                    .Select(v => new VerseDto
                    {
                        Book = v.Book,
                        Chapter = v.Chapter,
                        Verse = v.Verse,
                        Text = v.Text
                    })
                    .ToListAsync(ct);
            }, ct);

        // --- Freitext-Referenz (optional minimal) ---

        public Task<IReadOnlyList<VerseDto>> GetByReferenceAsync(string reference, CancellationToken ct = default)
        {
            // Minimal-Variante: Versucht "Book Chapter:From-To" zu lesen, sonst Kapitel komplett.
            // Für robustes Parsing baue deinen ScriptureParser / Map ein.
            if (TryParseSimple(reference, out var book, out var chapter, out var v1, out var v2))
            {
                if (v1 == null && v2 == null)
                    return GetChapterAsync(book, chapter, ct);

                var from = v1 ?? 1;
                var to = v2 ?? v1 ?? 1;
                return GetRangeAsync(book, chapter, from, to, ct);
            }
            throw new ArgumentException($"Ungültige Referenz: '{reference}'", nameof(reference));
        }

        // sehr einfache, tolerante Parser-Hilfe
        private static bool TryParseSimple(string input, out int book, out int chapter, out int? v1, out int? v2)
        {
            book = chapter = 0; v1 = v2 = null;
            if (string.IsNullOrWhiteSpace(input)) return false;

            // Erwartet ungefähr: "<book> <chapter>[:<v1>[-<v2>]]"
            // Beispiel: "62 1:1-5"  (nutze vorher deine Map, um "1. Johannes" -> 62 zu machen)
            var parts = input.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 1) return false;

            // Wenn du hier schon eine Buchnummer übergeben willst, reicht das:
            if (!int.TryParse(parts[0], out book)) return false;

            if (parts.Length >= 2)
            {
                var rest = parts[1];
                var chapAndVerses = rest.Split(':');
                if (!int.TryParse(chapAndVerses[0], out chapter)) return false;

                if (chapAndVerses.Length == 2)
                {
                    var vr = chapAndVerses[1].Split('-', StringSplitOptions.RemoveEmptyEntries);
                    if (vr.Length >= 1 && int.TryParse(vr[0], out var a)) v1 = a;
                    if (vr.Length >= 2 && int.TryParse(vr[1], out var b)) v2 = b;
                }
            }
            return book > 0 && chapter > 0;
        }
    }
}
