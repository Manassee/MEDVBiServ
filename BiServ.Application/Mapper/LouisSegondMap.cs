using MEDVBiServ.Domain.Enums;
using System.Text.RegularExpressions;
using static MEDVBiServ.Application.Mapper.ElberfelderMap;

namespace MEDVBiServ.Application.Mapper
{
    public static class LouisSegondMap
    {
        // Codes basieren auf gebräuchlichen frz. Abkürzungen; Aliasse decken akzentlose Formen & Kurzschreibweisen ab.
        private static readonly BookDef[] Books = new[]
        {
            new BookDef( 1, "Gen",  "Genèse",               Testament.AT, new[] { "genese", "gen" }),
            new BookDef( 2, "Ex",   "Exode",                Testament.AT, new[] { "ex" }),
            new BookDef( 3, "Lev",  "Lévitique",            Testament.AT, new[] { "levitique", "lev" }),
            new BookDef( 4, "Nb",   "Nombres",              Testament.AT, new[] { "nom", "nombres", "nb" }),
            new BookDef( 5, "Dt",   "Deutéronome",          Testament.AT, new[] { "deuteronome", "dt", "deut" }),
            new BookDef( 6, "Jos",  "Josué",                Testament.AT, new[] { "josue", "jos" }),
            new BookDef( 7, "Jug",  "Juges",                Testament.AT, new[] { "juges", "jug" }),
            new BookDef( 8, "Rt",   "Ruth",                 Testament.AT, new[] { "ruth", "rt" }),
            new BookDef( 9, "1S",   "1 Samuel",             Testament.AT, new[] { "1 samuel", "1sam", "1 s" }),
            new BookDef(10, "2S",   "2 Samuel",             Testament.AT, new[] { "2 samuel", "2sam", "2 s" }),
            new BookDef(11, "1R",   "1 Rois",               Testament.AT, new[] { "1 rois", "1r", "1 roi" }),
            new BookDef(12, "2R",   "2 Rois",               Testament.AT, new[] { "2 rois", "2r", "2 roi" }),
            new BookDef(13, "1Ch",  "1 Chroniques",         Testament.AT, new[] { "1 chroniques", "1ch" }),
            new BookDef(14, "2Ch",  "2 Chroniques",         Testament.AT, new[] { "2 chroniques", "2ch" }),
            new BookDef(15, "Esd",  "Esdras",               Testament.AT, new[] { "esdras", "esd" }),
            new BookDef(16, "Neh",  "Néhémie",              Testament.AT, new[] { "nehemie", "neh" }),
            new BookDef(17, "Est",  "Esther",               Testament.AT, new[] { "esther", "est" }),
            new BookDef(18, "Job",  "Job",                  Testament.AT, new[] { "job" }),
            new BookDef(19, "Ps",   "Psaumes",              Testament.AT, new[] { "psaumes", "ps", "psaume" }),
            new BookDef(20, "Pr",   "Proverbes",            Testament.AT, new[] { "prov", "proverbes", "pr" }),
            new BookDef(21, "Ec",   "Ecclésiaste",          Testament.AT, new[] { "ecclesiaste", "qohelet", "ec" }),
            new BookDef(22, "Ca",   "Cantique des cantiques",Testament.AT, new[] { "cantique", "cant", "ca", "cantique des cantiques" }),
            new BookDef(23, "Es",   "Ésaïe",                Testament.AT, new[] { "esaie", "esa", "es" }),
            new BookDef(24, "Jer",  "Jérémie",              Testament.AT, new[] { "jeremie", "jer" }),
            new BookDef(25, "Lam",  "Lamentations",         Testament.AT, new[] { "lamentations", "lam" }),
            new BookDef(26, "Ez",   "Ézéchiel",             Testament.AT, new[] { "ezechiel", "ezekiel", "ez" }),
            new BookDef(27, "Dan",  "Daniel",               Testament.AT, new[] { "daniel", "dan" }),
            new BookDef(28, "Os",   "Osée",                 Testament.AT, new[] { "osee", "os" }),
            new BookDef(29, "Jl",   "Joël",                 Testament.AT, new[] { "joel", "jl" }),
            new BookDef(30, "Am",   "Amos",                 Testament.AT, new[] { "amos", "am" }),
            new BookDef(31, "Ab",   "Abdias",               Testament.AT, new[] { "abdias", "ab" }),
            new BookDef(32, "Jon",  "Jonas",                Testament.AT, new[] { "jonas", "jon" }),
            new BookDef(33, "Mi",   "Michée",               Testament.AT, new[] { "michee", "mi", "mic" }),
            new BookDef(34, "Na",   "Nahum",                Testament.AT, new[] { "nahum", "na" }),
            new BookDef(35, "Ha",   "Habacuc",              Testament.AT, new[] { "habacuc", "hab", "ha" }),
            new BookDef(36, "So",   "Sophonie",             Testament.AT, new[] { "sophonie", "so", "soph" }),
            new BookDef(37, "Ag",   "Aggée",                Testament.AT, new[] { "aggee", "ag" }),
            new BookDef(38, "Za",   "Zacharie",             Testament.AT, new[] { "zacharie", "za", "zac" }),
            new BookDef(39, "Mal",  "Malachie",             Testament.AT, new[] { "malachie", "mal" }),

            new BookDef(40, "Mt",   "Matthieu",             Testament.NT, new[] { "matthieu", "mt", "mat" }),
            new BookDef(41, "Mc",   "Marc",                 Testament.NT, new[] { "marc", "mc" }),
            new BookDef(42, "Lc",   "Luc",                  Testament.NT, new[] { "luc", "lc" }),
            new BookDef(43, "Jn",   "Jean",                 Testament.NT, new[] { "jean", "jn", "evangile de jean", "ev jean" }),
            new BookDef(44, "Ac",   "Actes",                Testament.NT, new[] { "actes", "act", "ac" }),
            new BookDef(45, "Rm",   "Romains",              Testament.NT, new[] { "romains", "rom", "rm" }),
            new BookDef(46, "1Co",  "1 Corinthiens",        Testament.NT, new[] { "1 corinthiens", "1co", "1 co" }),
            new BookDef(47, "2Co",  "2 Corinthiens",        Testament.NT, new[] { "2 corinthiens", "2co", "2 co" }),
            new BookDef(48, "Ga",   "Galates",              Testament.NT, new[] { "galates", "ga", "gal" }),
            new BookDef(49, "Ep",   "Éphésiens",            Testament.NT, new[] { "ephesiens", "ep", "eph" }),
            new BookDef(50, "Ph",   "Philippiens",          Testament.NT, new[] { "philippiens", "php", "ph" }),
            new BookDef(51, "Col",  "Colossiens",           Testament.NT, new[] { "colossiens", "col" }),
            new BookDef(52, "1Th",  "1 Thessaloniciens",    Testament.NT, new[] { "1 thessaloniciens", "1th", "1 th" }),
            new BookDef(53, "2Th",  "2 Thessaloniciens",    Testament.NT, new[] { "2 thessaloniciens", "2th", "2 th" }),
            new BookDef(54, "1Tm",  "1 Timothée",           Testament.NT, new[] { "1 timothee", "1tm", "1 tm" }),
            new BookDef(55, "2Tm",  "2 Timothée",           Testament.NT, new[] { "2 timothee", "2tm", "2 tm" }),
            new BookDef(56, "Tt",   "Tite",                 Testament.NT, new[] { "tite", "tt" }),
            new BookDef(57, "Phm",  "Philémon",             Testament.NT, new[] { "philemon", "phm", "phlm" }),
            new BookDef(58, "He",   "Hébreux",              Testament.NT, new[] { "hebreux", "heb", "he" }),
            new BookDef(59, "Jc",   "Jacques",              Testament.NT, new[] { "jacques", "jc", "jac" }),
            new BookDef(60, "1P",   "1 Pierre",             Testament.NT, new[] { "1 pierre", "1p", "1 pi" }),
            new BookDef(61, "2P",   "2 Pierre",             Testament.NT, new[] { "2 pierre", "2p", "2 pi" }),
            new BookDef(62, "1Jn",  "1 Jean",               Testament.NT, new[] { "1 jean", "1jn", "1 jn" }),
            new BookDef(63, "2Jn",  "2 Jean",               Testament.NT, new[] { "2 jean", "2jn", "2 jn" }),
            new BookDef(64, "3Jn",  "3 Jean",               Testament.NT, new[] { "3 jean", "3jn", "3 jn" }),
            new BookDef(65, "Jud",  "Jude",                 Testament.NT, new[] { "jude" }),
            new BookDef(66, "Ap",   "Apocalypse",           Testament.NT, new[] { "apocalypse", "ap", "apoc" })
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
            => Regex.Replace(s.ToLowerInvariant(), @"[\s\.\-–—()'’]+", " ",RegexOptions.None,TimeSpan.FromMilliseconds(100)).Trim();

        public static string GetName(int id) => ById.TryGetValue(id, out var b) ? b.Name : $"Livre {id}";
        public static string GetCode(int id) => ById.TryGetValue(id, out var b) ? b.Code : $"L{id}";
        public static Testament GetTestament(int id) => ById.TryGetValue(id, out var b) ? b.T : id <= 39 ? Testament.AT : Testament.NT;

        public static bool TryGetId(string input, out int id)
        {
            id = 0;
            if (string.IsNullOrWhiteSpace(input)) return false;

            var key = Normalize(input);
            if (AliasToId.TryGetValue(key, out id)) return true;

            // Fallback: StartsWith (tolerant bei verkürzten Eingaben)
            var match = AliasToId.FirstOrDefault(kvp => kvp.Key.StartsWith(key, StringComparison.Ordinal));
            if (!match.Equals(default(KeyValuePair<string, int>))) { id = match.Value; return true; }

            return false;
        }

        public static IReadOnlyList<BookDef> GetAll() => Books;
    }
}
