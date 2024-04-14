using Microsoft.EntityFrameworkCore;
using OrleansAuctions.Blazor.Models;

namespace OrleansAuctions.Blazor.Infra;

public class AuctionContext : DbContext
{
    public AuctionContext(DbContextOptions<AuctionContext> options) : base(options)
    {
    }

    public DbSet<Auction> Auctions { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultContainer("Auctions");

        builder.Entity<Auction>()
            .ToContainer("Auctions")
            .HasPartitionKey(a => a.AuctionId)
            .HasNoDiscriminator();
    }
}