using Microsoft.EntityFrameworkCore;
using PlataformaMarcenaria.API.Entities;

namespace PlataformaMarcenaria.API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<BudgetRequest> BudgetRequests { get; set; }
    public DbSet<Bid> Bids { get; set; }
    public DbSet<Visit> Visits { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.UserType).HasConversion<string>();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETDATE()");
        });

        // BudgetRequest configuration
        modelBuilder.Entity<BudgetRequest>(entity =>
        {
            entity.HasOne(e => e.Client)
                .WithMany(u => u.BudgetRequests)
                .HasForeignKey(e => e.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Location)
                .WithMany()
                .HasForeignKey(e => e.LocationId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.Status).HasConversion<string>();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETDATE()");
        });

        // Bid configuration
        modelBuilder.Entity<Bid>(entity =>
        {
            entity.HasOne(e => e.Carpenter)
                .WithMany(u => u.Bids)
                .HasForeignKey(e => e.CarpenterId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.BudgetRequest)
                .WithMany(br => br.Bids)
                .HasForeignKey(e => e.BudgetRequestId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.Status).HasConversion<string>();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETDATE()");
        });

        // Visit configuration
        modelBuilder.Entity<Visit>(entity =>
        {
            entity.HasOne(e => e.Seller)
                .WithMany(u => u.Visits)
                .HasForeignKey(e => e.SellerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.BudgetRequest)
                .WithMany(br => br.Visits)
                .HasForeignKey(e => e.BudgetRequestId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.Status).HasConversion<string>();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETDATE()");
        });

        // Address configuration
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasOne(e => e.User)
                .WithMany(u => u.Addresses)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}

