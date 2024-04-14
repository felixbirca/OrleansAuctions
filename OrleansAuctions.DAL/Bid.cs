namespace OrleansAuctions.DAL;

public class Bid
{
    public Guid Id { get; set; }
    public Guid AuctionId { get; set; }
    public Guid UserId { get; set; }
    public DateTimeOffset BidDateTime { get; set; }
    public decimal Amount { get; set; }
}