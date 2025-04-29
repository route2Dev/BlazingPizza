using BlazingPizza.Services;
using Microsoft.AspNetCore.Components;

namespace BlazingPizza.ViewModels;

public class CheckoutViewModel(IHttpClientFactoryService httpClientFactoryService, OrderService orderService, NavigationManager navigationManager, ILogger<CheckoutViewModel> logger) 
{
    private readonly HttpClient _httpClient = httpClientFactoryService.CreateClient();

    public bool IsSubmitting { get; private set; }

    public Order CurrentOrder => orderService.CurrentOrder;

    public async Task PlaceOrderAsync()
    {
        IsSubmitting = true;

        var response = await _httpClient.PostAsJsonAsync("orders", CurrentOrder);

        if (!response.IsSuccessStatusCode)
        {
            var reason = await response.Content.ReadAsStringAsync();
            logger.LogError("Error submitting order: {REASON}", reason);

            return;
        }

        int newOrderId = await response.Content.ReadFromJsonAsync<int>();

        orderService.ClearOrder();
        navigationManager.NavigateTo("/");
    }
}