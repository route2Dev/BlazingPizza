using BlazingPizza.Components;
using BlazingPizza.Data;
using BlazingPizza.Services;
using BlazingPizza.ViewModels;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient("default");
builder.Services.AddScoped<IHttpClientFactoryService, HttpClientFactoryService>();

// builder.Services.AddSqlite<PizzaStoreContext>("Data Source=pizza.db");
builder.Services.AddDbContext<PizzaStoreContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PizzaStoreConnection")));


builder.Services.AddControllers();

// custom services
builder.Services.AddScoped<OrderService>();

// view models
builder.Services.AddScoped<HomeViewModel>();
builder.Services.AddScoped<CheckoutViewModel>();

var app = builder.Build();

app.UseStatusCodePagesWithReExecute("/not-found");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

// Initialize the database
var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var scope = scopeFactory.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PizzaStoreContext>();
    if (db.Database.EnsureCreated())
    {
        SeedData.Initialize(db);
    }
}

app.Run();
