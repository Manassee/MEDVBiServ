using MEDVBiServ.Application.Enums;
using Microsoft.AspNetCore.Http;

namespace MEDVBiServ.Application.Interfaces
{
    public interface ILanguageProvider
    {
        Translation Resolve(HttpContext httpContext);
        
    }
}
