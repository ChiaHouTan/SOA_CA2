namespace SOA_CA2.Services
{
    using SOA_CA2.Models;
    public interface IApiKeyService
    {
        ApiKey GetApiKey(string key);
    }
}
