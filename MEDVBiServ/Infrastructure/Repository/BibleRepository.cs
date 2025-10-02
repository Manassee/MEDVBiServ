// Infrastructure/Repository/BibleRepository{TContext}.cs
using MEDVBiServ.Application.Dtos;
using MEDVBiServ.Domain.Entities;
using MEDVBiServ.Infrastructure.Repository.interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

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


        private static readonly Regex _simpleRefRx = new(
            @"^\s*(?<book>\d+)(?:\s+(?<chapter>\d+)(?::(?<v1>\d+)(?:-(?<v2>\d+))?)?)?\s*$",
            RegexOptions.Compiled,
            matchTimeout: TimeSpan.FromMilliseconds(100));


        // sehr einfache, tolerante Parser-Hilfe
        private static bool TryParseSimple(string input, out int book, out int chapter, out int? v1, out int? v2)
        {
            book = chapter = 0; v1 = v2 = null;
            if (string.IsNullOrWhiteSpace(input)) return false;

            var m = _simpleRefRx.Match(input);
            if (!m.Success) return false;

            // Pflichtfelder
            if (!int.TryParse(m.Groups["book"].Value, out book)) return false;
            if (m.Groups["chapter"].Success && !int.TryParse(m.Groups["chapter"].Value, out chapter)) return false;

            // Optionale Verse
            if (m.Groups["v1"].Success && int.TryParse(m.Groups["v1"].Value, out var a)) v1 = a;
            if (m.Groups["v2"].Success && int.TryParse(m.Groups["v2"].Value, out var b)) v2 = b;

            return book > 0 && chapter > 0;
        }
    }
}
