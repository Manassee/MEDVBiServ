using MEDVBiServ.App.Application.Enums;

namespace MEDVBiServ.App.Application.Interfaces
{
    public interface ILanguageProvider
    {
        Translation Resolve(HttpContext httpContext);
    }
}
