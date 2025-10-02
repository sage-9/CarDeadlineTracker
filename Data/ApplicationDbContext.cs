using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using CarDeadlineTracker.Model;

namespace CarDeadlineTracker.Data;

public class ApplicationDbContext:DbContext
{
    public DbSet<Car> Cars { get; set; }
    public DbSet<RenewalItem> RenewalItems { get; set; }
    public DbSet<RepairLog> RepairLogs { get; set; }

    private readonly string _path;

    public ApplicationDbContext()
    {
        // Get the directory where the application's executable (.exe) is located.
        // This is the simplest and most common method for desktop apps.
        string appDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        
        // Combine the directory path with your database file name.
        // This will create a path like: "C:\path\to\published\app\car_manager.db"
        _path = Path.Combine(appDirectory, "car_manager.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // The Data Source now uses the dynamically generated path
        optionsBuilder.UseSqlite($"Data Source={_path}");
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