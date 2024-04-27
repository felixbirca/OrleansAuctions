using Microsoft.AspNetCore.Components;
using OrleansAuctions.Blazor.Services;
using OrleansAuctions.DAL;

namespace OrleansAuctions.Blazor.Components.Pages;

public partial class AuctionItem
{
    [Inject]
    private IAuctionService _auctionService { get; set; }
    [Inject]
    private IBiddingService _biddingService { get; set; }
    [Parameter]
    public string AuctionId { get; set; }
    [Inject] 
    private ILogger<AuctionItem> Logger { get; set; }
    public Auction Auction { get; set; }
    public decimal BidPrice { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        this.Auction = await _auctionService.GetAuction(AuctionId);
        this.BidPrice = Auction.StartingPrice + 100;
        Logger.LogInformation("Bid Price set");
    }

    private async Task Bid()
    {
        try
        {
            await _biddingService.AddBidAsync(Guid.Parse(AuctionId), Guid.NewGuid(), BidPrice);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
}