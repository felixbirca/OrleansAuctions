using Orleans;
namespace OrleansAuctions.Abstractions;

public interface IAuctionGrain : IGrainWithGuidKey
{
    public Task PlaceBid(decimal bidAmount, Guid userId, Guid auctionId);
    public Task<AuctionGrainState> GetState();
}