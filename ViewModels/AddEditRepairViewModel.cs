using System.Windows;
using System.Windows.Input;
using CarDeadlineTracker.Data;
using CarDeadlineTracker.Model;

namespace CarDeadlineTracker.ViewModels;

public class AddEditRepairViewModel : ViewModelBase
{
    
    private RepairLog _repairLog;
    
    public bool IsEditable => !_isEditing;

    public RepairLog SelectedRepairLog
    {
        get => _repairLog;
        set
        {
            _repairLog = value;
            OnPropertyChanged();
        }
    }
    
    private readonly bool _isEditing;
    public string CarNumberPlate { get; set; }

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public AddEditRepairViewModel(string carNumberPlate)
    {
        SelectedRepairLog = new RepairLog();
        _isEditing=false;
        CarNumberPlate = carNumberPlate;
        SelectedRepairLog.RepairDate = DateTime.Today;
        SaveCommand = new RelayCommand(SaveRepairAndClose);
        CancelCommand = new RelayCommand(CancelAndClose);
    }

    public AddEditRepairViewModel(string selectedCarNumberPlate, RepairLog selectedRepairLog)
    {
        _isEditing=true;
        SelectedRepairLog = selectedRepairLog;
        CarNumberPlate = selectedCarNumberPlate;
        SelectedRepairLog.RepairDate = DateTime.Today;
        
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