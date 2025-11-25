using System;
using MEDVBiServ.Application.Enums;
using MEDVBiServ.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;        // <- wichtig für GetRouteData()
using Microsoft.Extensions.Primitives;      // <- für StringValues

namespace MEDVBiServ.Application.Services
{
    public class LanguageProvider : ILanguageProvider
    {
        public Translation Resolve(HttpContext httpContext)
        {
            // 1) Route: /api/{lang}/...
            var routeData = httpContext.GetRouteData();
            if (routeData?.Values.TryGetValue("lang", out var rv) == true
                && rv is string lang1
                && Try(lang1, out var t1))
            {
                return t1;
            }

            // 2) Query: ?lang=fr
            if (httpContext.Request.Query.TryGetValue("lang", out StringValues q)
                && Try(q.ToString(), out var t2))
            {
                return t2;
            }

            // 3) Header: Accept-Language: fr-FR,fr;q=0.9,de-DE;q=0.8
            var al = httpContext.Request.Headers["Accept-Language"].ToString();
            if (!string.IsNullOrWhiteSpace(al))
            {
                if (al.StartsWith("fr", StringComparison.OrdinalIgnoreCase)) return Translation.Fr;
                if (al.StartsWith("de", StringComparison.OrdinalIgnoreCase)) return Translation.De;
            }

            // Fallback
            return Translation.De;

            static bool Try(string s, out Translation t)
            {
                switch (s?.Trim().ToLowerInvariant())
                {
                    case "de":
                    case "ger":
                    case "de-de":
                        t = Translation.De; return true;

                    case "fr":
                    case "fra":
                    case "fr-fr":
                        t = Translation.Fr; return true;

                    default:
                        t = Translation.De; return false;
                }
            }
        }
    }
}
