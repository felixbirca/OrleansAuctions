using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using OrleansAuctions.Abstractions;
using OrleansAuctions.Blazor.Services;

namespace OrleansAuctions.Blazor.Components.Pages;

public partial class AuctionItem
{
    [Inject]
    private IAuctionService _auctionService { get; set; }
    
    [Inject]
    private IBiddingService _biddingService { get; set; }
    
    [Inject] 
    private ILogger<AuctionItem> _logger { get; set; }
    
    [Inject]
    private LocalStorageService _localStorageService { get; set; }
    
    [Inject]
    private NavigationManager _navManager { get; set; }
    
    [Parameter]
    public string AuctionId { get; set; }

    private HubConnection? _hubConnection;
    
    public AuctionGrainState Auction { get; set; }
    public decimal BidPrice { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _logger.LogInformation("Initializing page, testing logs");
        
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(_navManager.ToAbsoluteUri("/auctionhub"))
            .Build();

        _hubConnection.On<decimal, Guid>("ReceiveMessage", (bidAmount, winnerId) =>
        {
            Auction.WinningBidAmount = bidAmount;
            Auction.Winner = winnerId;
            InvokeAsync(StateHasChanged);
        });
        
        await _hubConnection.StartAsync();
        await _hubConnection.SendAsync("JoinAuctionAsync", AuctionId);
    }

    protected override async Task OnParametersSetAsync()
    {
        this.Auction = await _auctionService.GetAuction(Guid.Parse(AuctionId));
        this.BidPrice = Auction.StartingPrice + 100;
        _logger.LogInformation("Bid Price set");
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await _localStorageService.SetItemAsync("userId", Guid.NewGuid().ToString());
    }
    
    private async Task Bid()
    {
        try
        {
            var userId = await _localStorageService.GetItemAsync("userId");
            await _biddingService.AddBidAsync(Guid.Parse(AuctionId), Guid.Parse(userId), BidPrice);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
}