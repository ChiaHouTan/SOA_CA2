using Microsoft.AspNetCore.Http;
using SOA_CA2.Services;
using System.Threading.Tasks;

namespace SOA_CA2.Middlewares
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string ApiKeyHeaderName = "X-Api-Key";

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IApiKeyService apiKeyService)
        {
            if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var potentialApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("API Key is missing.");
                return;
            }

            var apiKey = apiKeyService.GetApiKey(potentialApiKey);
            if (apiKey == null || !apiKey.IsActive)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Unauthorized API Key.");
                return;
            }

            // Attach role to HttpContext for later use
            context.Items["UserRole"] = apiKey.Role;
            await _next(context);
        }
    }
}
