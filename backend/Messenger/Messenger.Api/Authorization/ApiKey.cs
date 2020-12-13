
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Messenger.Api.Authorization
{
public class ApiKeyRequirement : IAuthorizationRequirement
    {
        public IReadOnlyList<string> ApiKeys { get; set; }

        public ApiKeyRequirement(IEnumerable<string> apiKeys)
        {
            ApiKeys = apiKeys?.ToList() ?? new List<string>();
        }
    }

    public class ApiKeyRequirementHandler : AuthorizationHandler<ApiKeyRequirement>
    {
        public const string API_KEY_HEADER_NAME = "X-API-KEY";
        private readonly IHttpContextAccessor httpContextAccessor;

        public ApiKeyRequirementHandler(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ApiKeyRequirement requirement)
        {
            SucceedRequirementIfApiKeyPresentAndValid(context, requirement);
            return Task.CompletedTask;
        }

        private void SucceedRequirementIfApiKeyPresentAndValid(AuthorizationHandlerContext context, ApiKeyRequirement requirement)
        {
            var apiKey = httpContextAccessor.HttpContext.Request.Headers[API_KEY_HEADER_NAME].FirstOrDefault();
            if (apiKey != null && requirement.ApiKeys.Any(requiredApiKey => apiKey == requiredApiKey))
            {
                context.Succeed(requirement);
            }
        }
    }
}