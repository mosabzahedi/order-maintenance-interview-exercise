using Microsoft.EntityFrameworkCore;

namespace OrderMaintenance.Data;

public class OrdersDbContext(DbContextOptions<OrdersDbContext> options) : DbContext(options)
{
    public DbSet<Order> Orders => Set<Order>();

    public DbSet<DemoUser> DemoUsers => Set<DemoUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var order = modelBuilder.Entity<Order>();

        order.Property(x => x.TrackingNumber).HasMaxLength(30);
        order.HasIndex(x => x.TrackingNumber).IsUnique();
        order.Property(x => x.CustomerDisplayName).HasMaxLength(100);
        order.Property(x => x.Status).HasMaxLength(20);
        order.Property(x => x.CustomerNote).HasMaxLength(500);

        order.HasOne<DemoUser>()
            .WithMany()
            .HasForeignKey(x => x.UpdatedByDemoUserId);

        modelBuilder.Entity<DemoUser>()
            .Property(x => x.SecretHash)
            .HasMaxLength(500);
    }
}
