// Create a file named ApiKeyService.cs in the Services folder
using SOA_CA2.Models;
using SOA_CA2.Services;
using System.Collections.Generic;
using System.Linq;

namespace SOA_CA2.Services
{
    public class ApiKeyService : IApiKeyService
    {
        private readonly List<ApiKey> _apiKeys = new()
        {
            new ApiKey { Key = "public-key-123", Role = "Public", CreatedOn = DateTime.UtcNow, IsActive = true },
            new ApiKey { Key = "admin-key-456", Role = "Admin", CreatedOn = DateTime.UtcNow, IsActive = true }
        };

        public ApiKey GetApiKey(string key)
        {
            return _apiKeys.FirstOrDefault(apiKey => apiKey.Key == key);
        }
    }
}
