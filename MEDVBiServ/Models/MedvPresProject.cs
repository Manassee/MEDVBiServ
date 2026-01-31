using System;
using System.Collections.Generic;
using MEDVBiServ.Models;

namespace MEDVBiServ.Models
{
    
    public class MedvPresProject
    {
        public string SchemaVersion { get; set; } = "1.0";
        public string Title { get; set; } = ""; // z.b Gottesdienst_31-01-2026
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string App { get; set; } = "MEDVBiServ";
        public string AppVersion { get; set; } = "1.0.0"; // TODO: Automatisch füllen
        public string Language { get; set; } = string.Empty;
        public List<MedvSlideItem> Slides { get; set; } = new();
    }
}
