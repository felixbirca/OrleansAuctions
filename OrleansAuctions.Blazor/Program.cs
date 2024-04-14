using Microsoft.EntityFrameworkCore;
using OrleansAuctions.Blazor.Components;
using OrleansAuctions.Blazor.Infra;
using OrleansAuctions.Blazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Host
    .UseOrleansClient(client =>
    {
        client.UseLocalhostClustering();
    })
    .ConfigureLogging(logging => logging.AddConsole());

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddDbContextFactory<AuctionContext>(options => options.UseCosmos(builder.Configuration.GetConnectionString("COSMOS"), "OrleansAuctions"));
builder.Services.AddScoped<IAuctionService, AuctionService>();
builder.Services.AddAntiforgery(options => { options.Cookie.Expiration = TimeSpan.Zero;});

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