using MEDVBiServ.Application.Dtos;
using MEDVBiServ.Domain.Entities;

namespace MEDVBiServ.Application.Mapper
{
    /// <summary>
    /// Provides methods for mapping Bible entity objects to data transfer objects (DTOs) representing Bible verses.
    /// </summary>
    public static class BibleVerseMapper
    {
        public static VerseDto ToDto(Bible entity, string bookName)
        {
            // Validierung der Eingabeparameter
            if (entity is null) throw new ArgumentNullException(nameof(entity));
            if (string.IsNullOrWhiteSpace(bookName)) bookName = string.Empty;

            // Wenn entity.Id z.B. long ist, ist VerseDto.Id idealerweise auch long.
            // Falls VerseDto.Id int bleiben muss, dann wenigstens checked cast:
            var id = checked((int)entity.Id);

            // Erstellen und Rückgabe des VerseDto-Objekts mit den entsprechenden Werten aus der Bible-Entität und dem Buchnamen

            return new VerseDto
            {
                Id = id,
                Book = entity.Book,
                BookName = bookName,
                Chapter = entity.Chapter,
                Verse = entity.Verse,
                Text = entity.Text
            };
        }
    }
}