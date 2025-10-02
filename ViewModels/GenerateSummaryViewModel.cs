using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using CarDeadlineTracker.Data;
using CarDeadlineTracker.Model;
using Microsoft.Win32;

namespace CarDeadlineTracker.ViewModels;

public class GenerateSummaryViewModel:ViewModelBase
{
    public Car SelectedCar { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string OutputPath { get; set; }
    
    
    private readonly List<RenewalItem> _originalRenewalItems;
    private readonly List<RepairLog> _originalRepairLogs;
    
    public ICommand GenerateSummaryCommand { get; set; }
    
    public GenerateSummaryViewModel(Car car,List<RenewalItem> renewalItems, List<RepairLog> repairLogs)
    { 
        GenerateSummaryCommand = new RelayCommand(GenerateSummary);
        SelectedCar = car;
        StartDate = new DateTime(DateTime.Today.Year, 1, 1);
        EndDate = DateTime.Today;
        
        // Store the original lists
        _originalRenewalItems = renewalItems;
        _originalRepairLogs = repairLogs;
    }
    
    private bool GetPath()
    {
        var saveWindow = new SaveFileDialog
        {
            FileName = $"CarSummary_{SelectedCar?.NumberPlate.Replace(" ", "_")}",
            DefaultExt = ".csv",
            Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*"
        };
        
        bool? result = saveWindow.ShowDialog();

        if (result == true)
        {
            OutputPath = saveWindow.FileName;
            return true;
        } 
        return false;
    }
    private void WriteCsvFile(List<RenewalItem> filteredRenewals, List<RepairLog> filteredRepairs)
    {
        //Calculate the total costs
        decimal totalRenewalCost = filteredRenewals.Sum(r => r.Cost);
        decimal totalRepairCost = filteredRepairs.Sum(r => r.Cost);
        decimal grandTotal = totalRenewalCost + totalRepairCost;
        
        //CSV Header for itemized records
        string HeaderRow = "Type,Name,Date,Cost";
        
        using (StreamWriter sw = new StreamWriter(OutputPath))
        {
            sw.WriteLine($"Summary for Car: {SelectedCar?.NumberPlate} ({SelectedCar?.Brand} {SelectedCar?.Make})");
            sw.WriteLine($"Period: {DateOnly.FromDateTime(StartDate)} to {DateOnly.FromDateTime(EndDate)}");
            sw.WriteLine();
            
            //ITEMIZED RECORDS
            sw.WriteLine("DETAILED ITEMS");
            sw.WriteLine(HeaderRow);
            
            //Write Renewal Items
            foreach (var item in filteredRenewals.OrderBy(a => a.DateOfRenewal))
            {
                DateOnly date = DateOnly.FromDateTime(item.DateOfRenewal.Date);
                string line = $"Renewal Item,{item.ItemName},{date},{item.Cost:F2}";
                sw.WriteLine(line);
            }
            
            //Write Repair Logs
            foreach (var log in filteredRepairs.OrderBy(i => i.RepairDate))
            {
                DateOnly date = DateOnly.FromDateTime(log.RepairDate.Date);
                string line = $"Repair Log,{log.RepairName},{date},{log.Cost:F2}";
                sw.WriteLine(line);
            }
            
            //SUMMARY TOTALS
            sw.WriteLine();
            sw.WriteLine("SUMMARY TOTALS");
            sw.WriteLine("Category,Count,Total Cost");
            
            sw.WriteLine($"Total Renewal Items,{filteredRenewals.Count},{totalRenewalCost:F2}");
            sw.WriteLine($"Total Repair Logs,{filteredRepairs.Count},{totalRepairCost:F2}");
            sw.WriteLine();
            sw.WriteLine($"GRAND TOTAL SPENT (Renewal + Repair),,{grandTotal:F2}");
        }
    }
    
    private void GenerateSummary(object Parameter)
    {
        if (!GetPath())
        {
            return;
        }
        
        
        var filteredRenewals = _originalRenewalItems
            .Where(a => a.DateOfRenewal.Date >= StartDate.Date && a.DateOfRenewal.Date <= EndDate.Date)
            .ToList();

        var filteredRepairs = _originalRepairLogs
            .Where(i => i.RepairDate.Date >= StartDate.Date && i.RepairDate.Date <= EndDate.Date)
            .ToList();
        
        
        WriteCsvFile(filteredRenewals, filteredRepairs);
        
    }
}
