namespace CarDeadlineTracker.Model;

public class RenewalItem
{
    public int Id { get; set; } // Primary key
    public string   ItemName { get; set; }
    public DateTime DateOfRenewal { get; set; }
    public DateTime DateOfExpiry { get; set; }
    public decimal  Cost { get; set; }
    public string Notes { get; set; }
    
    public bool IsDone { get; set; }

    // Foreign key to the Car model
    public string CarNumberPlate { get; set; } 
    public Car Car { get; set; }
}