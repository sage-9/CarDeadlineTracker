namespace CarDeadlineTracker.Model;

public class MaintenanceRecord
{
    public int Id { get; set; } // Primary key
    public DateTime MaintenanceDate { get; set; }
    public DateTime NextMaintenanceDueDate { get; set; }
    public string MaintenanceName { get; set; }
    public string Notes { get; set; }

    // Foreign key to the Car model
    public string CarNumberPlate { get; set; }
    public Car Car { get; set; }
}