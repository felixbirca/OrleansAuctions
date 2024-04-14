namespace OrleansAuctions.Blazor.Models;

public enum AuctionStatus
{
    Active,
    Completed,
    Cancelled
}

public class Auction
{
    public Guid AuctionId { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public decimal StartingPrice { get; set; }
    public DateTime StartTime { get; set; }
    public int Duration { get; set; }
    public AuctionStatus AuctionStatus { get; set; }
}