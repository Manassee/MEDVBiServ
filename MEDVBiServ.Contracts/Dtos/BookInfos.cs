using MEDVBiServ.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDVBiServ.Contracts.Dtos
{
    public record BookInfos
    {
        public int Id { get; init; }            // 1..66 (entspricht Bible.Book)
        public string Code { get; init; } = ""; // z.B. "Gen", "Ps", "Joh", "1Joh"
        public string Name { get; init; } = ""; // z.B. "1. Mose", "Psalmen", "Johannes
        public Testament Testament { get; init; }
        public int Chapters { get; init; }      // max. Kapitelnummer
    }
}
