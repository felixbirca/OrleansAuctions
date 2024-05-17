using Microsoft.EntityFrameworkCore;
using OrleansAuctions.DAL;
using OrleansAuctions.Abstractions;

namespace OrleansAuctions.Blazor.Services;

public class AuctionService : IAuctionService
{
    private readonly IDbContextFactory<AuctionContext> _dbContext;
    private readonly IClusterClient _client;

    public AuctionService(IDbContextFactory<AuctionContext> dbContext, IClusterClient client)
    {
        _dbContext = dbContext;
        _client = client;
    }

    public async Task CreateAuction(Auction auction)
    {
        await using var context = await _dbContext.CreateDbContextAsync();
        {
            try
            {
                await context.Database.EnsureCreatedAsync();
                var result = await context.Auctions.AddAsync(auction);
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }

    public async Task<List<Auction>> GetAuctions()
    {
        var auctions = new List<Auction>();
        
        await using var context = await _dbContext.CreateDbContextAsync();
        {
            try
            {
                await context.Database.EnsureCreatedAsync();
                auctions = await context.Auctions.OrderByDescending(x => x.EndTime).ToListAsync();
            }
            catch (Exception e)
            {
                
            }
        }
        return auctions;
    }

    public async Task<AuctionGrainState> GetAuction(Guid id)
    {
        var auctionGrain = _client.GetGrain<IAuctionGrain>(id);
        var auction = await auctionGrain.GetState();

        return auction;
    }
}