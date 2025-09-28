using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using CarDeadlineTracker.Data;
using CarDeadlineTracker.Model;
using CarDeadlineTracker.ViewModels;

public class AddEditRenewalItemViewModel : ViewModelBase
{
    private readonly bool _isEditing;

    
    public bool IsEditable => !_isEditing;
    
    private RenewalItem _selectedRenewalItem;
    
    private bool _isValid;
    
    private string _validStatement; 
    public string ValidStatement
    {
        get => _validStatement;
        set
        {
           
            _validStatement = value;
            
            OnPropertyChanged();
        }
    }

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
        SelectedRenewalItem.DateOfRenewal = DateTime.Today;
        SelectedRenewalItem.DateOfExpiry = DateTime.Today;
        _isEditing=true;
        CarNumberPlate = carNumberPlate;
        SaveCommand = new RelayCommand(SaveRenewalItemAndClose);
        CancelCommand = new RelayCommand(CancelAndClose);
    }
    
    public AddEditRenewalItemViewModel(string carNumberPlate)
    {
        SelectedRenewalItem = new RenewalItem();
        _isEditing = false;
        SelectedRenewalItem.DateOfRenewal = DateTime.Today;
        SelectedRenewalItem.DateOfExpiry = DateTime.Today;
        CarNumberPlate = carNumberPlate;
        SaveCommand = new RelayCommand(SaveRenewalItemAndClose);
        CancelCommand = new RelayCommand(CancelAndClose);
    }

   
    private void ValidateData(RenewalItem item)
    {
        _isValid = true;
        if(item.ItemName == null) _isValid = false;
        if(item.DateOfRenewal == null) item.DateOfRenewal = DateTime.Today;
        if (item.DateOfExpiry == null) item.DateOfExpiry = DateTime.Today.AddDays(1);
        if(item.Cost == null) item.Cost = 0;
        if(item.Notes == null) item.Notes = String.Empty;
    }

    private void SaveRenewalItemAndClose(object parameter)
    {
        // Link the new document to the correct car
        SelectedRenewalItem.CarNumberPlate = CarNumberPlate;
        ValidateData(SelectedRenewalItem);
        if (!_isEditing)
        {
            ValidStatement = "The data you entered is invalid";
        }
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