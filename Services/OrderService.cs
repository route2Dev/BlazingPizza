namespace BlazingPizza.Services;

public class OrderService
{
    public Order CurrentOrder { get; private set; } = new Order();

    public event Action? OnOrderChanged;

    public void AddPizzaToOrder(Pizza pizza)
    {
        CurrentOrder.Pizzas.Add(pizza);
        NotifyStateChanged();
    }

    public void RemovePizzaFromOrder(Pizza pizza)
    {
        CurrentOrder.Pizzas.Remove(pizza);
        NotifyStateChanged();
    }

    private void NotifyStateChanged()
    {
        OnOrderChanged?.Invoke();
    }

    public void ClearOrder()
    {
        CurrentOrder = new Order();
        NotifyStateChanged();
    }
}
