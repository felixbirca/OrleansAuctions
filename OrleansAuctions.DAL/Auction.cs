namespace OrleansAuctions.DAL;

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
    public AuctionStatus AuctionStatus { get; set; }
    public DateTime EndTime { get; set; }
}