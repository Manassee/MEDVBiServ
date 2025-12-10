using BiServ.Infrastructure.Repository.interfaces;
using MEDVBiServ.Application.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiServ.Infrastructure.Repository
{
    public class BibleRepository<TContext> : IBibleRepository where TContext : DbContext
    {
        private readonly TContext? _context; 


        public BibleRepository(TContext context) 
        { 
            _context = context;

        }

       /* private async Task<TResult>WithDb<TResult>(Func<TContext, Task<TResult>> action, CancellationToken ct)
        {
            if (_context == null)
            {
                throw new InvalidOperationException("Datenbank Context ist nicht verfügbar.");
            }
            else
            {
                await using var db = await _context.CreateDbContextAsync(ct);
            }
            return await action(_context);
        }*/



        public Task<IReadOnlyList<BookInfos>> GetAllBooksAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<BookInfos?> GetBookAsync(int book, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<VerseDto>> GetByReferenceAsync(string reference, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<VerseDto>> GetChapterAsync(int book, int chapter, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<int>> GetChapterNumbersAsync(int book, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<VerseDto>> GetRangeAsync(int book, int chapter, int fromVerse, int toVerse, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<VerseDto?> GetVerseAsync(int book, int chapter, int verse, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<VerseDto>> SearchAsync(string term, int? book = null, int limit = 50, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
