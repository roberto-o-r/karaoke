using Krk.Components;
using Krk.Repositories;
using Krk.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<SongService>();
builder.Services.AddSingleton<FavoritesService>();

var connectionString = builder.Configuration["Cosmos:ConnectionString"];
if (connectionString == null)
{
    throw new ArgumentNullException("Cosmos:ConnectionString");
}

builder.Services.AddSingleton(new QueueRepository(connectionString, "Karaoke"));
builder.Services.AddSingleton(new FavoritesRepository(connectionString, "Karaoke"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
