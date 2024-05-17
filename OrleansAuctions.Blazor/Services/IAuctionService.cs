using OrleansAuctions.DAL;
using OrleansAuctions.Abstractions;

namespace OrleansAuctions.Blazor.Services;

public interface IAuctionService
{
    public Task CreateAuction(Auction auction);
    public Task<List<Auction>> GetAuctions();
    public Task<AuctionGrainState> GetAuction(Guid id);
}