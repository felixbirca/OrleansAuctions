using Microsoft.AspNetCore.Components;
using OrleansAuctions.Blazor.Services;
using OrleansAuctions.DAL;

namespace OrleansAuctions.Blazor.Components.Pages;

public partial class Home
{
    [Inject]
    private IAuctionService _auctionService { get; set; }

    public List<Auction> auctions { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
    }
    
    protected override async Task OnParametersSetAsync()
    {
        auctions = await _auctionService.GetAuctions();
    }
}