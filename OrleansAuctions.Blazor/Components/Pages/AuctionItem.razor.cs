using Microsoft.AspNetCore.Components;
using OrleansAuctions.Blazor.Models;
using OrleansAuctions.Blazor.Services;

namespace OrleansAuctions.Blazor.Components.Pages;

public partial class AuctionItem
{
    [Inject]
    private IAuctionService _auctionService { get; set; }
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

    private void Bid()
    {
        Logger.LogInformation(BidPrice.ToString());
    }
}