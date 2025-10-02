using MEDVBiServ.Domain.Enums;
using System.Text.RegularExpressions;

namespace MEDVBiServ.Application.Mapper
{
    public static class ElberfelderMap
    {
        public sealed record BookDef(int Id, string Code, string Name, Testament T, string[] Aliases);


        private static readonly BookDef[] Books = new[]
        {
            new BookDef(1,  "Gen", "1. Mose", Testament.AT, new[] { "Genesis", "1 mose", "1. mose", "1mo", "1 mo" }),
            new BookDef(2,  "Exod", "2. Mose", Testament.AT, new[] { "Exodus", "2 mose", "2. mose", "2mo", "2 mo" }),
            new BookDef(3,  "Lev", "3. Mose", Testament.AT, new[] { "Leviticus", "3 mose", "3. mose", "3mo", "3 mo" }),
            new BookDef(4,  "Num", "4. Mose", Testament.AT, new[] { "Numeri", "4 mose", "4. mose", "4mo", "4 mo" }),
            new BookDef(5,  "Deut", "5. Mose", Testament.AT, new[] { "Deuteronomium", "5 mose", "5. mose", "5mo", "5 mo" }),
            new BookDef(6,  "Josh", "Josua", Testament.AT, new[] { "Josua" }),
            new BookDef(7,  "Judg", "Richter", Testament.AT, new[] { "Richter" }),
            new BookDef(8,  "Ruth", "Rut", Testament.AT, new[] { "Rut" }),
            new BookDef(9,  "1Sam", "1. Samuel", Testament.AT, new[] { "1 samuel", "1. samuel", "1sa", "1 sa" }),
            new BookDef(10, "2Sam", "2. Samuel", Testament.AT, new[] { "2 samuel", "2. samuel", "2sa", "2 sa" }),
            new BookDef(11, "1Kgs", "1. Könige", Testament.AT, new[] { "1 könige", "1. könige", "1ki", "1 ki" }),
            new BookDef(12, "2Kgs", "2. Könige", Testament.AT, new[] { "2 könige", "2. könige", "2ki", "2 ki" }),
            new BookDef(13, "1Chr", "1. Chronik", Testament.AT, new[] { "1 chronik", "1. chronik", "1ch", "1 ch" }),
            new BookDef(14, "2Chr", "2. Chronik", Testament.AT, new[] { "2 chronik", "2. chronik", "2ch", "2 ch" }),
            new BookDef(15, "Ezra", "Esra", Testament.AT, new[] { "Esra" }),
            new BookDef(16, "Neh", "Nehemia", Testament.AT, new[] { "Nehemia" }),
            new BookDef(17, "Esth", "Ester", Testament.AT, new[] { "Ester" }),
            new BookDef(18, "Job", "Hiob", Testament.AT, new[] { "Hiob" }),
            new BookDef(19, "Ps", "Psalmen", Testament.AT, new[] { "Psalmen", "Psalter" }),
            new BookDef(20, "Prov", "Sprüche", Testament.AT, new[] { "Sprüche", "Sprichwörter" }),
            new BookDef(21, "Eccl", "Prediger", Testament.AT, new[] { "Prediger", "Kohelet" }),
            new BookDef(22, "Song", "Hohelied", Testament.AT, new[] { "Hohelied", "Lied der Lieder" }),
            new BookDef(23, "Isa", "Jesaja", Testament.AT, new[] { "Jesaja" }),
            new BookDef(24, "Jer", "Jeremia", Testament.AT, new[] { "Jeremia" }),
            new BookDef(25, "Lam", "Klagelieder", Testament.AT, new[] { "Klagelieder" }),
            new BookDef(26, "Ezek", "Hesekiel", Testament.AT, new[] { "Hesekiel" }),
            new BookDef(27, "Dan", "Daniel", Testament.AT, new[] { "Daniel" }),
            new BookDef(28, "Hos", "Hosea", Testament.AT, new[] { "Hosea" }),
            new BookDef(29, "Joel", "Joel", Testament.AT, new[] { "Joel" }),
            new BookDef(30, "Amos", "Amos", Testament.AT, new[] { "Amos" }),
            new BookDef(31, "Obad", "Obadja", Testament.AT, new[] { "Obadja" }),
            new BookDef(32, "Jonah", "Jona", Testament.AT, new[] { "Jona" }),
            new BookDef(33, "Mic", "Micha", Testament.AT, new[] { "Micha" }),
            new BookDef(34, "Nah", "Nahum", Testament.AT, new[] { "Nahum" }),
            new BookDef(35, "Hab", "Habakuk", Testament.AT, new[] { "Habakuk" }),
            new BookDef(36, "Zeph", "Zefanja", Testament.AT, new[] { "Zefanja" }),
            new BookDef(37, "Hag", "Haggai", Testament.AT, new[] { "Haggai" }),
            new BookDef(38, "Zech", "Sacharja", Testament.AT, new[] { "Sacharja" }),
            new BookDef(39, "Mal", "Maleachi", Testament.AT, new[] { "Maleachi" }),
            new BookDef(40, "Matt", "Matthäus", Testament.NT, new[] { "Matthäus", "Matthaeus" }),
            new BookDef(41, "Mark", "Markus", Testament.NT, new[] { "Markus" }),
            new BookDef(42, "Luke", "Lukas", Testament.NT, new[] { "Lukas" }),
            new BookDef(43, "John", "Johannes", Testament.NT, new[] { "Johannes" }),
            new BookDef(44, "Acts", "Apostelgeschichte", Testament.NT, new[] { "Apostelgeschichte", "Apg" }),
            new BookDef(45, "Rom", "Römer", Testament.NT, new[] { "Römer" }),
            new BookDef(46, "1Cor", "1. Korinther", Testament.NT, new[] { "1 korinther", "1. korinther", "1co", "1 co" }),
            new BookDef(47, "2Cor", "2. Korinther", Testament.NT, new[] { "2 korinther", "2. korinther", "2co", "2 co" }),
            new BookDef(48, "Gal", "Galater", Testament.NT, new[] { "Galater" }),
            new BookDef(49, "Eph", "Epheser", Testament.NT, new[] { "Epheser" }),
            new BookDef(50, "Phil", "Philipper", Testament.NT, new[] { "Philipper" }),
            new BookDef(51, "Col", "Kolosser", Testament.NT, new[] { "Kolosser" }),
            new BookDef(52, "1Thess", "1. Thessalonicher", Testament.NT, new[] { "1 thessalonicher", "1. thessalonicher", "1th", "1 th" }),
            new BookDef(53, "2Thess", "2. Thessalonicher", Testament.NT, new[] { "2 thessalonicher", "2. thessalonicher", "2th", "2 th" }),
            new BookDef(54, "1Tim", "1. Timotheus", Testament.NT, new[] { "1 timotheus", "1. timotheus", "1ti", "1 ti" }),
            new BookDef(55, "2Tim", "2. Timotheus", Testament.NT, new[] { "2 timotheus", "2. timotheus", "2ti", "2 ti" }),
            new BookDef(56, "Titus", "Titus", Testament.NT, new[] { "Titus" }),
            new BookDef(57, "Phlm", "Philemon", Testament.NT, new[] { "Philemon", "Phlm" }),
            new BookDef(58, "Heb", "Hebräer", Testament.NT, new[] { "Hebräer", "Hebraeer" }),
            new BookDef(59, "Jas", "Jakobus", Testament.NT, new[] { "Jakobus" }),
            new BookDef(60, "1Pet", "1. Petrus", Testament.NT, new[] { "1 petrus", "1. petrus", "1pe", "1 pe" }),
            new BookDef(61, "2Pet", "2. Petrus", Testament.NT, new[] { "2 petrus", "2. petrus", "2pe", "2 pe" }),
            new BookDef(62, "1John", "1. Johannes", Testament.NT, new[] { "1 johannes", "1. johannes", "1jo", "1 jo" }),
            new BookDef(63, "2John", "2. Johannes", Testament.NT, new[] { "2 johannes", "2. johannes", "2jo", "2 jo" }),
            new BookDef(64, "3John", "3. Johannes", Testament.NT, new[] { "3 johannes", "3. johannes", "3jo", "3 jo" }),
            new BookDef(65, "Jude", "Judas", Testament.NT, new[] { "Judas" }),
            new BookDef(66, "Rev", "Offenbarung", Testament.NT, new[] { "Offenbarung", "Apokalypse" })
        };

        private static readonly Dictionary<int, BookDef> ById = Books.ToDictionary(b => b.Id);
        private static readonly Dictionary<string, int> AliasToId = BuildAliasMap();

        private static Dictionary<string, int> BuildAliasMap()
        {
            var map = new Dictionary<string, int>(StringComparer.Ordinal);
            foreach (var b in Books)
            {
                Add(b.Code, b.Id);
                Add(b.Name, b.Id);
                foreach (var a in b.Aliases) Add(a, b.Id);
            }
            return map;

            static string K(string s) => Normalize(s);
            void Add(string s, int id)
            {
                var k = K(s);
                if (!map.ContainsKey(k)) map[k] = id;
            }
        }

        private static string Normalize(string s)
            => Regex.Replace(s.ToLowerInvariant(), @"[\s\.\-–—()]+", " ",RegexOptions.None,TimeSpan.FromMilliseconds(100)).Trim(); // Timeout setzen gegen Regex-DoS

        public static string GetName(int id) => ById.TryGetValue(id, out var b) ? b.Name : $"Buch {id}";
        public static string GetCode(int id) => ById.TryGetValue(id, out var b) ? b.Code : $"B{id}";
        public static Testament GetTestament(int id) => ById.TryGetValue(id, out var b) ? b.T : id <= 39 ? Testament.AT : Testament.NT;

        public static bool TryGetId(string input, out int id)
        {
            id = 0;
            if (string.IsNullOrWhiteSpace(input)) return false;

            var key = Normalize(input);
            if (AliasToId.TryGetValue(key, out id)) return true;

            // Fallback: starts-with-Matching
            var match = AliasToId.FirstOrDefault(kvp => kvp.Key.StartsWith(key, StringComparison.Ordinal));
            if (!match.Equals(default(KeyValuePair<string, int>))) { id = match.Value; return true; }

            return false;
        }

        public static IReadOnlyList<BookDef> GetAll() => Books;


    }
}
