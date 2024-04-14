using Microsoft.EntityFrameworkCore;

namespace OrleansAuctions.DAL;

public class AuctionContext : DbContext
{
    public AuctionContext(DbContextOptions<AuctionContext> options) : base(options)
    {
    }
    
    public DbSet<Auction> Auctions { get; set; }
    public DbSet<Bid> Bids { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseCosmos("", "Auctions");
    

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultContainer("Auctions");

        builder.Entity<Auction>()
            .ToContainer("Auctions")
            .HasPartitionKey(a => a.Category)
            .HasNoDiscriminator();

        builder.Entity<Bid>()
            .ToContainer("Bids")
            .HasPartitionKey(b => b.AuctionId)
            .HasNoDiscriminator();
    }
}