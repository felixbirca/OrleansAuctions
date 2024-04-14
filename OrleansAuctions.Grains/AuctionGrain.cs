using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Orleans.Runtime;
using OrleansAuctions.DAL;
using OrleansAuctions.Grains.Models;

namespace OrleansAuctions.Grains;

public class AuctionGrain : Grain, IAuctionGrain
{
    private readonly IPersistentState<AuctionGrainState> _auction;
    private readonly ILogger<AuctionGrain> _logger;
    private readonly IDbContextFactory<AuctionContext> _dbContextFactory;
    private AuctionContext _dbContext;

    public AuctionGrain(
        ILogger<AuctionGrain> logger, 
        [PersistentState(stateName:"Auctions", storageName:"OrleansAuctions")]
        IPersistentState<AuctionGrainState> auction, 
        IDbContextFactory<AuctionContext> dbContextFactory)
    {
        _logger = logger;
        _dbContextFactory = dbContextFactory;
    }

    public override async Task OnActivateAsync(CancellationToken _)
    {
        _dbContext = await _dbContextFactory.CreateDbContextAsync();
    }

    public async Task PlaceBid(decimal bidAmount, Guid userId, Guid auctionId)
    {
        await _dbContext.Database.EnsureCreatedAsync();

        await _dbContext.Bids.AddAsync(new Bid
        {
            Id = Guid.NewGuid(),
            AuctionId = auctionId,
            UserId = userId,
            Amount = bidAmount,
            BidDateTime = DateTimeOffset.Now
        });

        await _dbContext.SaveChangesAsync();
    }

    public Task<AuctionGrainState> GetAuctionDetails()
    {
        return Task.FromResult(_auction.State);
    }
}