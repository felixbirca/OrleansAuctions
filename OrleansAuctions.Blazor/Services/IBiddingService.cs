namespace OrleansAuctions.Blazor.Services;

public interface IBiddingService
{
    Task AddBidAsync(Guid auctionId, Guid userId, decimal amount);
}