using Microsoft.AspNetCore.SignalR.Client;
using OrleansAuctions.Abstractions;

namespace OrleansAuctions.Silo;

public class SignalRClient : ISignalRClient
{
    private HubConnection _connection;

    public SignalRClient(string hubUrl)
    {
        _connection = new HubConnectionBuilder()
            .WithUrl(hubUrl)
            .Build();
    }

    public async Task ConnectAsync()
    {
        await _connection.StartAsync();
    }

    public async Task SendMessageAsync(string target, params object[] args)
    {
        await _connection.InvokeAsync(target, args);
    }

    public async Task SendNewWinningBetMessage(string auctionId, decimal bidAmount, string winnerId)
    {
        await _connection.InvokeAsync("UpdateAuctionAsync", auctionId, bidAmount, winnerId);
    }

    public async Task DisconnectAsync()
    {
        await _connection.StopAsync();
    }
}
