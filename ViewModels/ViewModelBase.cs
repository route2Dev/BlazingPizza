namespace BlazingPizza.ViewModels;

public class ViewModelBase 
{
    // in interactive server mode the OnInitializedAsync gets called 2x because of prerender.
    public bool Initialized { get; private set; } = false;

    public virtual Task InitializeAsync()
    {
        Initialized = true;
        return Task.CompletedTask;
    }
}
