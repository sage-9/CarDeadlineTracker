using Microsoft.EntityFrameworkCore;
using CarDeadlineTracker.Model;

namespace CarDeadlineTracker.Data;

public class ApplicationDbContext:DbContext
{
    public DbSet<Car> Cars { get; set; }
    public DbSet<RenewalItem> RenewalItems { get; set; }
    public DbSet<RepairLog> RepairLogs { get; set; }

    private readonly string path =@"C:\Users\AbdulrahmanLadipo\RiderProjects\CarDeadlineTracker\car_manager.db";

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={path}");
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>()
            .HasKey(c => c.NumberPlate);
        
        modelBuilder.Entity<RenewalItem>()
            .HasOne(d => d.Car)
            .WithMany(c => c.RenewalItems)
            .HasForeignKey(d => d.CarNumberPlate);

        modelBuilder.Entity<RepairLog>()
            .HasOne(r => r.Car)
            .WithMany(c => c.RepairLogs)
            .HasForeignKey(r => r.CarNumberPlate);
    }
    
}