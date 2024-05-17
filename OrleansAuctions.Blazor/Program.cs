using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using OrleansAuctions.Blazor.Components;
using OrleansAuctions.Blazor.Hubs;
using OrleansAuctions.Blazor.Services;
using OrleansAuctions.DAL;

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
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});
builder.Services.AddScoped<IAuctionService, AuctionService>();
builder.Services.AddScoped<IBiddingService, BiddingService>();
builder.Services.AddScoped<LocalStorageService>();
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
app.UseResponseCompression();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.MapHub<AuctionHub>("/auctionhub");

app.Run();