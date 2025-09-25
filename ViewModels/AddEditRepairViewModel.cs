using System.Windows;
using System.Windows.Input;
using CarDeadlineTracker.Data;
using CarDeadlineTracker.Model;

namespace CarDeadlineTracker.ViewModels;

public class AddEditRepairViewModel : ViewModelBase
{
    public RepairLog SelectedRepairLog { get; set; }
    
    private readonly bool _isEditing;
    public string CarNumberPlate { get; set; }

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public AddEditRepairViewModel(string carNumberPlate)
    {
        _isEditing=true;
        CarNumberPlate = carNumberPlate;
        SaveCommand = new RelayCommand(SaveRepairAndClose);
        CancelCommand = new RelayCommand(CancelAndClose);
    }

    public AddEditRepairViewModel(string selectedCarNumberPlate, RepairLog selectedRepairLog)
    {
        _isEditing=true;
        SelectedRepairLog = selectedRepairLog;
        CarNumberPlate = selectedCarNumberPlate;
        SaveCommand = new RelayCommand(SaveRepairAndClose);
        CancelCommand = new RelayCommand(CancelAndClose);
    }

    private void SaveRepairAndClose(object parameter)
    {
        SelectedRepairLog.CarNumberPlate = CarNumberPlate;
        SelectedRepairLog.Notes = SelectedRepairLog.Notes?? string.Empty;
        SelectedRepairLog.RepairDescription = SelectedRepairLog.RepairDescription?? string.Empty;
        // Link the new document to the correct car
       
        using (var dbContext = new ApplicationDbContext())
        {
            if (_isEditing)
            {
                dbContext.RepairLogs.Update(SelectedRepairLog); 
            }
            else
            {
                dbContext.RepairLogs.Add(SelectedRepairLog);
            }
            
            
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