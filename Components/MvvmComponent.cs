using BlazingPizza.ViewModels;
using Microsoft.AspNetCore.Components;

namespace BlazingPizza.Components;

public class MvvmComponent<T> : ComponentBase where T : ViewModelBase
{
    [Inject]
    public required T ViewModel { get; set; }

    protected override async Task OnInitializedAsync()
    {
        if (!ViewModel.Initialized)
        {
            await ViewModel.InitializeAsync();
        }

        await base.OnInitializedAsync();
    }
}