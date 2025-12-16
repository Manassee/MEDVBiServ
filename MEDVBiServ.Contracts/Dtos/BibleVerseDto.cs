using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDVBiServ.Contracts.Dtos
{
    public sealed class BibleVerseDto
    {
        public int Id { get; set; }
        public string Book { get; set; } = string.Empty;
        public int Chapter { get; set; }
        public int Verse { get; set; }
        public string Text { get; set; } = string.Empty;
        public string Langu { get; init; } = string.Empty;
    }
}
