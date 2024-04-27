using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrleansAuctions.DAL;

try
{
    using IHost host = await StartSiloAsync();
    Console.WriteLine("\n\n Press enter to terminate... \n\n");
    Console.ReadLine();

    await host.StopAsync();

    return 0;
}
catch (Exception ex)
{
    Console.WriteLine(ex);
    return 1;
}


static async Task<IHost> StartSiloAsync()
{
    IConfigurationRoot configuration = new ConfigurationBuilder()
        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
        .AddJsonFile("appsettings.Development.json", false, true)
        .AddEnvironmentVariables()
        .Build();
    
    string cosmosConnectionString = configuration["ConnectionStrings:COSMOS"] ?? string.Empty;

    
    var builder = Host
        .CreateDefaultBuilder()
        .UseOrleans((context, silo) =>
        {
            silo
                .UseLocalhostClustering()
                .ConfigureLogging(logging => logging.AddConsole())
                .ConfigureServices(services =>
                    {
                        services.AddDbContextFactory<AuctionContext>(options => options.UseCosmos(cosmosConnectionString, "OrleansAuctions"));
                    }
                );
        });


    var host = builder.Build();
    await host.StartAsync();

    return host;
}