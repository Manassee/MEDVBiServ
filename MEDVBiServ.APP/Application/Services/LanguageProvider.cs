using MEDVBiServ.App.Application.Enums;
using MEDVBiServ.App.Application.Interfaces;

namespace MEDVBiServ.App.Application.Services
{
    public class LanguageProvider : ILanguageProvider
    {
        public Translation Resolve(HttpContext httpContext)
        {
            // 1) Route: /api/{lang}/...
            if (httpContext.Request.RouteValues.TryGetValue("lang", out var rv) &&
                rv is string lang1 && Try(lang1, out var t1)) return t1;

            // 2) Query: ?lang=fr
            if (httpContext.Request.Query.TryGetValue("lang", out var q) &&
                Try(q.ToString(), out var t2)) return t2;

            // 3) Header: Accept-Language
            var al = httpContext.Request.Headers.AcceptLanguage.ToString().ToLowerInvariant();
            if (al.StartsWith("fr")) return Translation.Fr;
            if (al.StartsWith("de")) return Translation.De;

            // Fallback
            return Translation.De;

            static bool Try(string s, out Translation t)
            {
                switch (s.ToLowerInvariant())
                {
                    case "de": case "ger": case "de-de": t = Translation.De; return true;
                    case "fr": case "fra": case "fr-fr": t = Translation.Fr; return true;
                    default: t = Translation.De; return false;
                }
            }
        }
    }
}
