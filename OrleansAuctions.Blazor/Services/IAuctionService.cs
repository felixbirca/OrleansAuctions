using OrleansAuctions.DAL;

namespace OrleansAuctions.Blazor.Services;

public interface IAuctionService
{
    public Task CreateAuction(Auction auction);
    public Task<List<Auction>> GetAuctions();
    public Task<Auction> GetAuction(string id);
}