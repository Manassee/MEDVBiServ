namespace MEDVBiServ.Models
{
    public class MedvSlideItem
    {
        public string Type { get; set; } = "verse";         // "verse" oder "annotation"
        public string Display { get; set; } = string.Empty; // "Joh 3:16"
        public string Text { get; set; } = string.Empty;    // Vers-Text
    }
}
