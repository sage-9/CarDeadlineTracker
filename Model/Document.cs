namespace CarDeadlineTracker.Model;

public class Document
{
    public int Id { get; set; } // Primary key
    public string DocumentName { get; set; }
    public DateTime ExpirationDate { get; set; }
    public DateTime RenewalDate { get; set; }
    public decimal RenewalCost { get; set; }
    public string Notes { get; set; }

    // Foreign key to the Car model
    public string CarNumberPlate { get; set; } 
    public Car Car { get; set; }
}