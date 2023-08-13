namespace Bookify.Web2.Services
{
    public interface IEmailBodyBuilder
    {
        string GetEmailBody(string template, Dictionary<string, string> placeHolders);

    }
}
