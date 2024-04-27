using System.Security.AccessControl;
using OrleansAuctions.Abstractions;

namespace OrleansAuctions.Blazor.Services;

public class BiddingService : IBiddingService
{
    private readonly IClusterClient _client;

    public BiddingService(IClusterClient client)
    {
        _client = client;
    }

    public async Task AddBidAsync(Guid auctionId, Guid userId, decimal amount)
    {
        var auctionGrain = _client.GetGrain<IAuctionGrain>(auctionId);
        await auctionGrain.PlaceBid(amount, userId, auctionId);
    }
}