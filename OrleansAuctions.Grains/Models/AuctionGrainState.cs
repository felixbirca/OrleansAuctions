using OrleansAuctions.DAL;

namespace OrleansAuctions.Grains.Models;

public class AuctionGrainState
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Category { get; set; }
    public string Description { get; set; }
    public decimal StartingPrice { get; set; }
    public DateTime StartTime { get; set; }
    public int Duration { get; set; }
    public AuctionStatus AuctionStatus { get; set; }
    public decimal LastBidAmount { get; set; }
}