using Microsoft.EntityFrameworkCore;

namespace POCArgos.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<OrderStatus> OrderStatuses { get; set; } = null!;
    public DbSet<ShippingMethod> ShippingMethods { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Order>()
            .Property(o => o.RowVersion)
            .IsRowVersion();

        modelBuilder.Entity<OrderStatus>().HasData(
            new OrderStatus { Id = 1, Name = "Pending" },
            new OrderStatus { Id = 2, Name = "Processing" },
            new OrderStatus { Id = 3, Name = "Shipped" },
            new OrderStatus { Id = 4, Name = "Delivered" }
        );

        modelBuilder.Entity<ShippingMethod>().HasData(
            new ShippingMethod { Id = 1, Name = "Standard" },
            new ShippingMethod { Id = 2, Name = "Express" },
            new ShippingMethod { Id = 3, Name = "Next Day" }
        );
    }
}
