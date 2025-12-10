using MEDVBiServ.Application.Enums;
using MEDVBiServ.Application.Interfaces;
using Microsoft.Extensions.Primitives;
using System;
using System.Net.Http;

namespace MEDVBiServ.Application.Services
{
    public class LanguageProvider : ILanguageProvider
    {
        public Translation Resolve(string? routeLang, string? queryLang, string? headerLang)
        {
            // 1) Route
            if (Try(routeLang, out var t1)) return t1;

            // 2) Query
            if (Try(queryLang, out var t2)) return t2;

            // 3) Header
            if (!string.IsNullOrWhiteSpace(headerLang))
            {
                if (headerLang.StartsWith("fr", StringComparison.OrdinalIgnoreCase)) return Translation.Fr;
                if (headerLang.StartsWith("de", StringComparison.OrdinalIgnoreCase)) return Translation.De;
            }

            return Translation.De;
        }

        private static bool Try(string? s, out Translation t)
        {
            switch (s?.Trim().ToLowerInvariant())
            {
                case "de":
                case "de-de":
                case "ger":
                    t = Translation.De; return true;

                case "fr":
                case "fr-fr":
                case "fra":
                    t = Translation.Fr; return true;
            }

            t = Translation.De;
            return false;
        }
    }

}
