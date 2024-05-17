namespace OrleansAuctions.Abstractions;

public interface ISignalRClient
{
    Task ConnectAsync();
    
    Task SendMessageAsync(string target, params object[] args);
    
    Task SendNewWinningBetMessage(string auctionId, decimal bidAmount, string winnerId);

    Task DisconnectAsync();
}