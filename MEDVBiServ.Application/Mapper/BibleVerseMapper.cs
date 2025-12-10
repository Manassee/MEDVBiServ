using MEDVBiServ.Application.Dtos;
using MEDVBiServ.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDVBiServ.Application.Mapper
{
    public static class BibleVerseMapper
    {
        public static VerseDto ToDto(Bible entity, string bookName)
        {
            return new VerseDto
            {
                Id = (int)entity.Id,
                Book = entity.Book,
                BookName = bookName,
                Chapter = entity.Chapter,
                Verse = entity.Verse,
                Text = entity.Text
            };
        }
    }
}
