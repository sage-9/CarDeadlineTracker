using System.Windows;
using System.Windows.Input;
using CarDeadlineTracker.Data;
using CarDeadlineTracker.Model;

namespace CarDeadlineTracker.ViewModels;

public class AddEditRepairViewModel : ViewModelBase
{
    public RepairLog RepairLog { get; set; } = new RepairLog();
    public string CarNumberPlate { get; set; }

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public AddEditRepairViewModel(string carNumberPlate)
    {
        CarNumberPlate = carNumberPlate;
        SaveCommand = new RelayCommand(SaveRepairAndClose);
        CancelCommand = new RelayCommand(CancelAndClose);
    }

    private void SaveRepairAndClose(object parameter)
    {
        // Link the new document to the correct car
        RepairLog.CarNumberPlate = CarNumberPlate;
        RepairLog.Notes = RepairLog.Notes?? string.Empty;
        RepairLog.RepairDescription = RepairLog.RepairDescription?? string.Empty;
        using (var dbContext = new ApplicationDbContext())
        {
            
            dbContext.RepairLogs.Add(RepairLog);
            dbContext.SaveChanges();
        }

        if (parameter is Window window)
        {
            window.Close();
        }
    }

    private void CancelAndClose(object parameter)
    {
        if (parameter is Window window)
        {
            window.Close();
        }
    }
}