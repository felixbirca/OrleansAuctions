using Microsoft.EntityFrameworkCore;
using OrleansAuctions.DAL;

namespace OrleansAuctions.Blazor.Services;

public class AuctionService : IAuctionService
{
    private readonly IDbContextFactory<AuctionContext> _dbContext;

    public AuctionService(IDbContextFactory<AuctionContext> dbContext)
    {
        _dbContext = dbContext;
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
                auctions = await context.Auctions.OrderByDescending(x => x.StartTime).ToListAsync();
            }
            catch (Exception e)
            {
                
            }
        }
        return auctions;
    }

    public async Task<Auction> GetAuction(string id)
    {
        Auction auction = null; 
        
        await using var context = await _dbContext.CreateDbContextAsync();
        {
            try
            {
                await context.Database.EnsureCreatedAsync();
                auction = await context.Auctions.Where(x => x.AuctionId == Guid.Parse(id)).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                
            }
        }

        return auction;
    }
}