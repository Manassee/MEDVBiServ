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
    }
}
