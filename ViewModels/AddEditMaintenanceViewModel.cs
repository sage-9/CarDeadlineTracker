using System.Windows;
using System.Windows.Input;
using CarDeadlineTracker.Data;
using CarDeadlineTracker.Model;

namespace CarDeadlineTracker.ViewModels;

public class AddEditMaintenanceViewModel : ViewModelBase
{
    public MaintenanceRecord MaintenanceRecord { get; set; } = new MaintenanceRecord();
    public string CarNumberPlate { get; set; }

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public AddEditMaintenanceViewModel(string carNumberPlate)
    {
        CarNumberPlate = carNumberPlate;
        SaveCommand = new RelayCommand(SaveMaintenanceAndClose);
        CancelCommand = new RelayCommand(CancelAndClose);
    }

    private void SaveMaintenanceAndClose(object parameter)
    {
        // Link the new document to the correct car
        MaintenanceRecord.CarNumberPlate = CarNumberPlate;
        MaintenanceRecord.Notes = MaintenanceRecord.Notes?? string.Empty;
        using (var dbContext = new ApplicationDbContext())
        {
            
            dbContext.MaintenanceRecords.Add(MaintenanceRecord);
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