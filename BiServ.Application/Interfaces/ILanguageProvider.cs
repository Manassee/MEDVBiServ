using MEDVBiServ.Application.Enums;


namespace MEDVBiServ.Application.Interfaces
{
    public interface ILanguageProvider
    {
        Translation Resolve(string? routeLang, string? queryLang, string? headerLang);
    }

}
