using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Web;
using System.Net;
using Microsoft.Graph;

namespace webapp.Pages
{
    [AuthorizeForScopes(ScopeKeySection = "MicrosoftGraph:Scopes")]
    public class IndexModel : PageModel
    {
        private readonly GraphServiceClient _graphServiceClient;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger, GraphServiceClient graphServiceClient)
        {
            _logger = logger;
            _graphServiceClient = graphServiceClient;
        }

        public async Task OnGet()
        {
            using (_logger.BeginScope("{0} {1}", nameof(IndexModel), nameof(OnGet)))
            {
                var user = await _graphServiceClient.Me
                    .Request()
                    .GetAsync();
                _logger.LogInformation(user.UserPrincipalName);
                ViewData["GraphApiResult"] = user.DisplayName;
            }
        }
    }
}