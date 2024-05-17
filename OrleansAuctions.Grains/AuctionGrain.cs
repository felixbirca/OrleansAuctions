using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrleansAuctions.Abstractions;
using OrleansAuctions.DAL;

namespace OrleansAuctions.Grains;

public class AuctionGrain : Grain, IAuctionGrain
{
    private readonly ILogger<AuctionGrain> _logger;
    private readonly IDbContextFactory<AuctionContext> _dbContextFactory;
    private readonly ISignalRClient _signalRClient;
    private AuctionContext _dbContext;
    private AuctionGrainState _state;
    
    public AuctionGrain(ILogger<AuctionGrain> logger, IDbContextFactory<AuctionContext> dbContextFactory, ISignalRClient signalRClient)
    {
        _logger = logger;
        _dbContextFactory = dbContextFactory;
        _signalRClient = signalRClient;
    }

    public override async Task OnActivateAsync(CancellationToken ct)
    {
        _dbContext = await _dbContextFactory.CreateDbContextAsync(ct);
        await _dbContext.Database.EnsureCreatedAsync(ct);

        var grainId = this.GetPrimaryKey();
        
        var auction = await _dbContext.Auctions.FindAsync(grainId);

        if (auction != null)
        {
            var winningBid = await _dbContext.Bids.Where(b => b.AuctionId == auction.AuctionId)
                                                  .OrderByDescending(b => b.Amount)
                                                  .FirstOrDefaultAsync(ct);
            
            _state = new AuctionGrainState
            {
                Id = auction.AuctionId,
                AuctionStatus = auction.AuctionStatus,
                Category = "",
                Description = auction.Description,
                WinningBidAmount = winningBid?.Amount ?? auction.StartingPrice,
                Winner = winningBid?.UserId,
                EndTime = auction.EndTime,
                StartingPrice = auction.StartingPrice,
                Title = auction.Title
            };
        }
        
        await _signalRClient.ConnectAsync();
        
        await base.OnActivateAsync(ct);
    }

    public async Task<AuctionGrainState> GetState()
    {
        return await Task.FromResult(_state);
    }

    public async Task PlaceBid(decimal bidAmount, Guid userId, Guid auctionId)
    {
        await _dbContext.Bids.AddAsync(new Bid
        {
            Id = Guid.NewGuid(),
            AuctionId = auctionId,
            UserId = userId,
            Amount = bidAmount,
            BidDateTime = DateTimeOffset.Now
        });

        if (_state.WinningBidAmount < bidAmount)
        {
            _state.WinningBidAmount = bidAmount;
            _state.Winner = userId;

            var timeLeft = _state.EndTime - DateTime.Now;
            if (timeLeft.Seconds < 10)
            {
                _state.EndTime += timeLeft + TimeSpan.FromSeconds(11 - timeLeft.Seconds);
            }
            
            await _signalRClient.SendNewWinningBetMessage(auctionId.ToString(), bidAmount, userId.ToString());
        }

        var time = DateTimeOffset.Now;
        await _dbContext.SaveChangesAsync();
        var endTime = DateTimeOffset.Now;
        
        _logger.LogInformation($"Database write took {(endTime - time).Milliseconds} ms");
    }
}