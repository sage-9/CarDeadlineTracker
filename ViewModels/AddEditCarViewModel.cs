// ViewModels/AddEditRenewalItemViewModel.cs

using System.Windows;
using System.Windows.Input;
using CarDeadlineTracker.Data;
using CarDeadlineTracker.Model;

namespace CarDeadlineTracker.ViewModels;

public class AddEditCarViewModel : ViewModelBase
{
    private readonly bool _isEditing;
    public Car Car { get; set; }
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

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

    private void SaveCarAndClose(object parameter)
    {
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