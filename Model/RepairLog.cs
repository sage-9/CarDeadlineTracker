namespace CarDeadlineTracker.Model;

public class RepairLog
{
    public int Id { get; set; } // Primary key
    public string RepairName { get; set; }
    public DateTime RepairDate { get; set; }
    
    public decimal Cost { get; set; }
    public string RepairDescription { get; set; }
    public string Notes { get; set; }

    // Foreign key to the Car model
    public string CarNumberPlate { get; set; }
    public Car Car { get; set; }
    
}