using System.Windows;
using System.Windows.Input;
using CarDeadlineTracker.Data;
using CarDeadlineTracker.Model;
using CarDeadlineTracker.ViewModels;

public class AddEditRenewalItemViewModel : ViewModelBase
{
    private readonly bool _isEditing;
    private RenewalItem _selectedRenewalItem;

    public RenewalItem SelectedRenewalItem
    {
        get =>_selectedRenewalItem;
        set
        {
            _selectedRenewalItem = value;
            OnPropertyChanged();
        }
    }
    public string CarNumberPlate { get; set; }
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public AddEditRenewalItemViewModel(string carNumberPlate, RenewalItem selectedRenewalItem)
    {
        SelectedRenewalItem = selectedRenewalItem;
        _isEditing=true;
        CarNumberPlate = carNumberPlate;
        SaveCommand = new RelayCommand(SaveRenewalItemAndClose);
        CancelCommand = new RelayCommand(CancelAndClose);
    }
    
    public AddEditRenewalItemViewModel(string carNumberPlate)
    {
        SelectedRenewalItem = new RenewalItem();
        _isEditing = false;
        CarNumberPlate = carNumberPlate;
        SaveCommand = new RelayCommand(SaveRenewalItemAndClose);
        CancelCommand = new RelayCommand(CancelAndClose);
    }

    private void SaveRenewalItemAndClose(object parameter)
    {
        // Link the new document to the correct car
        SelectedRenewalItem.CarNumberPlate = CarNumberPlate;
        
        SelectedRenewalItem.Notes = SelectedRenewalItem.Notes ?? string.Empty;

        using (var dbContext = new ApplicationDbContext())
        {
            if (_isEditing)
            {
               dbContext.RenewalItems.Update(SelectedRenewalItem); 
            }
            else
            {
                dbContext.RenewalItems.Add(SelectedRenewalItem);
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