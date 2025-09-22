using Microsoft.EntityFrameworkCore;
using CarDeadlineTracker.Model;

namespace CarDeadlineTracker.Data;

public class ApplicationDbContext:DbContext
{
    public DbSet<Car> Cars { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; }
    public DbSet<RepairLog> RepairLogs { get; set; }

    private string path =@"C:\Users\AbdulrahmanLadipo\RiderProjects\CarDeadlineTracker\car_manager.db";

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={path}");
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>()
            .HasKey(c => c.NumberPlate);
        
        modelBuilder.Entity<Document>()
            .HasOne(d => d.Car)
            .WithMany(c => c.Documents)
            .HasForeignKey(d => d.CarNumberPlate);

        modelBuilder.Entity<MaintenanceRecord>()
            .HasOne(m => m.Car)
            .WithMany(c => c.MaintenanceRecords)
            .HasForeignKey(m => m.CarNumberPlate);

        modelBuilder.Entity<RepairLog>()
            .HasOne(r => r.Car)
            .WithMany(c => c.RepairLogs)
            .HasForeignKey(r => r.CarNumberPlate);
    }
    
}