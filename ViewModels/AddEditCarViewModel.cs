using System.Windows;
using System.Windows.Input;
using CarDeadlineTracker.Data;
using CarDeadlineTracker.Model;

namespace CarDeadlineTracker.ViewModels;

public class AddEditCarViewModel : ViewModelBase
{
    private readonly bool _isEditing;

    public bool IsEditable => !_isEditing;
    public Car Car { get; set; }
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }
    bool _isValid;
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


    public AddEditCarViewModel()
    {
        Car = new Car();
        _isEditing = false;
        SaveCommand = new RelayCommand(SaveCarAndClose);
        CancelCommand = new RelayCommand(CancelAndClose);
    }
    
    public AddEditCarViewModel(Car carToEdit)
    {
        Car = carToEdit;
        _isEditing = true;
        SaveCommand = new RelayCommand(SaveCarAndClose);
        CancelCommand = new RelayCommand(CancelAndClose);
    }

    private void ValidateData(Car car)
    { 
        _isValid = true;
       if(car.NumberPlate == null) _isValid = false;
       if (car.BranchLocation == null) car.BranchLocation = "Lagos";
       if (car.Brand == null) _isValid = false;
       if (car.Make == null) _isValid = false;
       if(car.Mileage == null) car.Mileage = 0;
    }

    private void SaveCarAndClose(object parameter)
    {
        ValidateData(Car);
        if (!_isValid)
        {
            ValidStatement = "The data you entered is invalid";
            return;
        }
        
        using (var dbContext = new ApplicationDbContext())
        {
            if (_isEditing)
            {
                dbContext.Cars.Update(Car);
            }
            else
            {
                dbContext.Cars.Add(Car);
            }
            dbContext.SaveChanges();
        }

        // Now, close the window.
        // We cast the parameter to a Window type.
        if (parameter is Window window)
        {
            window.Close();
        }
    }

    private void CancelAndClose(object parameter)
    {
        // Just close the window without saving anything.
        if (parameter is Window window)
        {
            window.Close();
        }
    }

}