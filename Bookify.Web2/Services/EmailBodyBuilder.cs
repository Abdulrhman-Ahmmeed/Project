namespace Bookify.Web2.Services
{
    public class EmailBodyBuilder : IEmailBodyBuilder
    {
        private readonly IWebHostEnvironment _webHost;

        public EmailBodyBuilder(IWebHostEnvironment webHost)
        {
            _webHost = webHost;
        }

        public string GetEmailBody(string template,Dictionary<string,string>placeHolders)
        {
            var path = $"{_webHost.WebRootPath}/templates/{template}.html";
            StreamReader stream = new(path);
            var templateContent = stream.ReadToEnd();
            stream.Close();
            foreach (var placeHolder in placeHolders)
                templateContent = templateContent.Replace($"[{placeHolder.Key}]", placeHolder.Value);

            return templateContent;
        }
    }
}
