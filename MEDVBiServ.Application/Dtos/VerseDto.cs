using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDVBiServ.Application.Dtos
{
    public class VerseDto
    {
        public int Id { get; set; }          // DB-Id
        public int Book { get; set; }        // Buchnummer 1–66
        public string BookName { get; set; } = "";  // für Anzeige: "Matthäus", "Jean", ...
        public int Chapter { get; set; }
        public int Verse { get; set; }
        public string Text { get; set; } = "";
        
    }
}
