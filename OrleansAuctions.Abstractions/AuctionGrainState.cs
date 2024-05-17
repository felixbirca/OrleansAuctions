using OrleansAuctions.DAL;

namespace OrleansAuctions.Abstractions;

[GenerateSerializer]
public class AuctionGrainState
{
    [Id(0)]
    public Guid Id { get; set; }
    [Id(1)]
    public string Title { get; set; }
    [Id(2)]
    public string Category { get; set; }
    [Id(3)]
    public string Description { get; set; }
    [Id(4)]
    public decimal StartingPrice { get; set; }
    [Id(5)]
    public DateTime EndTime { get; set; }
    [Id(6)]
    public AuctionStatus AuctionStatus { get; set; }
    [Id(7)]
    public decimal WinningBidAmount { get; set; }
    [Id(8)]
    public Guid? Winner { get; set; }
}