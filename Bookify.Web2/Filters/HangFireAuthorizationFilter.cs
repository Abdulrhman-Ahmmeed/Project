using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace Bookify.Web2.Filters
{
    public class HangFireAuthorizationFilter : IDashboardAuthorizationFilter
    {

        private string _Policy;

        public HangFireAuthorizationFilter(string policy)
        {
            _Policy = policy;
        }

        public bool Authorize([NotNull] DashboardContext context)
        {
            var HttpContext = context.GetHttpContext();
            var authServices = HttpContext.RequestServices.GetRequiredService<IAuthorizationService>();
            var IsAuthorized = authServices.AuthorizeAsync(HttpContext.User, _Policy)
                                            .ConfigureAwait(false)
                                            .GetAwaiter()
                                            .GetResult()
                                            .Succeeded;
            return IsAuthorized;
        }
    }
}
