using Microsoft.AspNetCore.Components;

namespace BlazingPizza.Services;

public interface IHttpClientFactoryService
{
    HttpClient CreateClient();    
}

public class HttpClientFactoryService(NavigationManager navigationManager, IHttpClientFactory httpClientFactory) : IHttpClientFactoryService
{
    public HttpClient CreateClient()
    {
        var client = httpClientFactory.CreateClient("default");
        client.BaseAddress = new Uri(navigationManager.BaseUri);

        return client;
    }
}