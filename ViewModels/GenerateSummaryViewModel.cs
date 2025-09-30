using System.IO;
using System.Windows.Input;
using CarDeadlineTracker.Data;
using CarDeadlineTracker.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;

namespace CarDeadlineTracker.ViewModels;

public class GenerateSummaryViewModel:ViewModelBase
{
    public Car SelectedCar { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public String OutputPath { get; set; }
    private List<RenewalItem> _renewalItems;
    private List<RepairLog> _repairLogs;
    
    public ICommand GenerateSummaryCommand { get; set; }
    public GenerateSummaryViewModel(List<RenewalItem> renewalItems, List<RepairLog> repairLogs)
    { 
        GenerateSummaryCommand = new RelayCommand(GenerateSummary);
        //get a list of items and Repairs (separate)
        _renewalItems = renewalItems;
        _repairLogs = repairLogs;
    }
    private void FilterList()
    {
        foreach (var a in _renewalItems)
        {
            if (!(a.DateOfRenewal - StartDate > TimeSpan.Zero && EndDate - a.DateOfRenewal > TimeSpan.Zero))
            {
                _renewalItems.Remove(a);
            }
        }

        foreach (var repairLog in _repairLogs)
        {
            if (!(repairLog.RepairDate - StartDate > TimeSpan.Zero && EndDate - repairLog.RepairDate > TimeSpan.Zero))
            {
                _repairLogs.Remove(repairLog);
                
            }
        }
    }
    private void WriteCsvFile()
    {
        string HeaderRow = "Name,Date,Cost";
        using (StreamWriter sw = new StreamWriter(OutputPath))
        {
            sw.WriteLine(HeaderRow);
            foreach (var a in _renewalItems)
            {
                string line = $"{a.ItemName},{a.DateOfRenewal},{a.Cost}";
                sw.WriteLine(line);
            }
            sw.WriteLine(",,");
            foreach (var i in _repairLogs)
            {
                string line = $"{i.RepairName},{i.RepairDate},{i.Cost}";
                sw.WriteLine(line);
            }
        }
    }
    void GetPath()
    {
        
        var saveWindow = new SaveFileDialog();
        saveWindow.FileName = "Summary";
        saveWindow.DefaultExt = ".csv";
        saveWindow.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
        bool? result = saveWindow.ShowDialog();

        if (result == true)
        {
            OutputPath = saveWindow.FileName;
        }
    }
    
    //create a csv file at the specified path using the name format of ("path + name.csv")
    //add each element to the csv(select their Name, date, and cost)
    private void GenerateSummary(object Parameter)
    {
        GetPath();
        FilterList();
        WriteCsvFile();
    }
}