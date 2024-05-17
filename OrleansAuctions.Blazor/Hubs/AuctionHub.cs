using Microsoft.AspNetCore.SignalR;
using OrleansAuctions.Abstractions;

namespace OrleansAuctions.Blazor.Hubs;

public class AuctionHub : Hub
{
    public async Task JoinAuctionAsync(string auctionId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, auctionId);
    }

    public async Task UpdateAuctionAsync(string auctionId, decimal bidAmount, string winnerId)
    {
        await Clients.Group(auctionId).SendAsync("ReceiveMessage", bidAmount, winnerId);
    }
}