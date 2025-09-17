using System.Collections.Generic;
namespace CarDeadlineTracker.Model;

public class Car
{
    // The primary key is the number plate, not an integer ID
    public string NumberPlate { get; set; } 
    public string Brand { get; set; }
    public string Make { get; set; }
    public string BranchLocation { get; set; }

    // Navigation properties
    public List<Document> Documents { get; set; }
    public List<MaintenanceRecord> MaintenanceRecords { get; set; }
    public List<RepairLog> RepairLogs { get; set; }
    
}