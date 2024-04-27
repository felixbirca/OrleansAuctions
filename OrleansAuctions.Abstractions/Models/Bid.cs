using Orleans;
// using System.Text.Json.Serialization;

namespace OrleansAuctions.Abstractions.Models;

[GenerateSerializer]
public class Bid
{
    [Id(0)]
    public Guid Id { get; set; }
    [Id(1)]
    public Guid AuctionId { get; set; }
    [Id(2)]
    public Guid UserId { get; set; }
    [Id(3)]
    public decimal Amount { get; set; }
}