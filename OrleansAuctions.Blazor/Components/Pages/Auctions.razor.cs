using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using OrleansAuctions.Blazor.Services;
using OrleansAuctions.DAL;

namespace OrleansAuctions.Blazor.Components.Pages;

[RequireAntiforgeryToken]
public partial class Auctions
{
    [Inject]
    private IAuctionService _auctionService { get; set; }
    
    [SupplyParameterFromForm] 
    private Auction? Model { get; set; }
    
    private EditContext? editContext;


    protected override void OnInitialized()
    {
        base.OnInitialized();
        Model ??= new Auction();
        editContext = new EditContext(Model);
    }

    private async Task SubmitForm(EditContext editContext)
    {
        await _auctionService.CreateAuction(Model);
    }
}