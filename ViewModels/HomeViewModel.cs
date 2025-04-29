
using BlazingPizza.Services;

namespace BlazingPizza.ViewModels;

public class HomeViewModel(IHttpClientFactoryService httpClientFactoryService, OrderService orderService): ViewModelBase
{
    private readonly HttpClient _httpClient = httpClientFactoryService.CreateClient();

    private readonly OrderService _orderService = orderService;

    public Order CurrentOrder => _orderService.CurrentOrder;

    public Pizza? CurrentPizza {get; private set;} = null;

    public List<PizzaSpecial> Specials { get; private set; } = [];

    public bool ShowConfigureDialog { get; private set; } = false;

    public async Task LoadSpecials()
    {
        Specials = await _httpClient.GetFromJsonAsync<List<PizzaSpecial>>("specials") ?? [];
    }

    public void ShowConfigurePizzaDialog(PizzaSpecial special)
    {
        CurrentPizza = new Pizza
        {
            Special = special,
            SpecialId = special.Id,
            Size = Pizza.DefaultSize,
            Toppings = []
        };     

        ShowConfigureDialog = true;
    }

    public void CancelConfigurePizzaDialog()
    {
        CurrentPizza = null;
        ShowConfigureDialog = false;
    }

    public void ConfirmConfigurePizzaDialog()
    {
        CurrentOrder.Pizzas.Add(CurrentPizza!);
        CurrentPizza = null;

        ShowConfigureDialog = false;
    }

    public void AddPizza(Pizza pizza)
    {
        _orderService.AddPizzaToOrder(pizza);
    }

    public void RemovePizza(Pizza pizza)
    {
        _orderService.RemovePizzaFromOrder(pizza);
    }

    public override async Task InitializeAsync()
    {
        await LoadSpecials();
        
        await base.InitializeAsync();
    }
}
