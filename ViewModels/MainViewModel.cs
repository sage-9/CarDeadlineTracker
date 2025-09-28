using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using CarDeadlineTracker.Data;
using CarDeadlineTracker.Model;
using CarDeadlineTracker.Views;
using Microsoft.EntityFrameworkCore;

namespace CarDeadlineTracker.ViewModels;

public class MainViewModel : ViewModelBase
{
    private Model.Car _selectedCar;

    public ObservableCollection<Car> Cars { get; set; } = new ObservableCollection<Car>();
    public ObservableCollection<RenewalItem> UpcomingDeadlines { get; set; } = new ObservableCollection<RenewalItem>();
    public ObservableCollection<RenewalItem> OverdueDeadlines { get; set; } = new ObservableCollection<RenewalItem>();

    public ICommand ToggleDoneCommand { get; }
    public ICommand DeleteCarCommand { get; }
    public ICommand AddCarCommand { get; }
    public ICommand ViewCarCommand { get; }
    public ICommand EditCarCommand { get; }

    public Model.Car SelectedCar
    {
        get => _selectedCar;
        set
        {
            _selectedCar = value;
            OnPropertyChanged();
        }
    }

    public MainViewModel()
    {
        AddCarCommand = new RelayCommand(AddCar);
        ViewCarCommand = new RelayCommand(ViewCar, CanEditCar);
        DeleteCarCommand = new RelayCommand(DeleteCar, CanEditCar);
        EditCarCommand = new RelayCommand(EditCar, CanEditCar);
        ToggleDoneCommand = new RelayCommand(ToggleDocumentDone);
        LoadCars();
    }

    private void AddCar(object parameter)
    {
        // Create an instance of the new view (the add/edit window)
        var addCarWindow = new AddEditCarView();
    
        // Create an instance of the new view model
        var viewModel = new AddEditCarViewModel();
    
        // Set the view's DataContext to the new view model
        addCarWindow.DataContext = viewModel;
    
        // Display the window as a modal dialog.
        // This blocks the main window until the dialog is closed.
        addCarWindow.ShowDialog();
    
        // When the dialog is closed, we need to refresh the list of cars.
        // This ensures the new car appears in the main window.
        LoadCars();
    }
    
    

    private void ViewCar(object parameter)
    {
        if (SelectedCar == null) return;

        var carDetailsWindow = new CarDetailsView();
    
        // Pass the selected Car object to the new view model
        var viewModel = new CarDetailsViewModel(SelectedCar);
    
        carDetailsWindow.DataContext = viewModel;
    
        carDetailsWindow.ShowDialog();
    
        // Refresh the main list to reflect any potential changes
        LoadCars();
    }

    private void EditCar(object parameter)
    {
        if (SelectedCar == null) return;
        var editCarWindow = new AddEditCarView();
        var viewModel = new AddEditCarViewModel(SelectedCar);
        
        editCarWindow.DataContext = viewModel;
        
        editCarWindow.ShowDialog();
        
        LoadCars();
        
        
    }

    private bool CanEditCar(object parameter)
    {
        return SelectedCar != null;
    }
    
    private void DeleteCar(object parameter)
    {
        if (SelectedCar == null) return;

        // Add a confirmation dialog for safety
        var result = MessageBox.Show($"Are you sure you want to delete the car with number plate {SelectedCar.NumberPlate}?", 
            "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        if (result == MessageBoxResult.Yes)
        {
            using (var dbContext = new ApplicationDbContext())
            {
                // The key part of the DELETE operation
                dbContext.Cars.Remove(SelectedCar);
                dbContext.SaveChanges();
            }
            // Refresh the UI list
            LoadCars();
        }
    }
    
    private void ToggleDocumentDone(object parameter)
    {
        if (parameter is RenewalItem documentToToggle)
        {
            // Save the change to the database
            using (var dbContext = new ApplicationDbContext())
            {
                dbContext.RenewalItems.Update(documentToToggle);
                dbContext.SaveChanges();
            }
        }
    }


    private void LoadCars()
    {
        Cars.Clear();
        UpcomingDeadlines.Clear();
        OverdueDeadlines.Clear();
        using (var dbContext = new ApplicationDbContext())
        {
            if (dbContext.Database != null)
            {
                var cars = dbContext.Cars.Include(c => c.RenewalItems).ToList();
                
                foreach (var car in cars)
                {
                    Cars.Add(car);

                    foreach (var doc in car.RenewalItems)
                    {
                        // Only track deadlines that are NOT marked as done
                        if (!doc.IsDone)
                        {
                            TimeSpan timeUntilExpiration = doc.DateOfExpiry - DateTime.Today;

                            if (timeUntilExpiration.TotalDays < 0)
                            {
                                // Overdue (Negative days)
                                OverdueDeadlines.Add(doc);
                            }
                            else if (timeUntilExpiration.TotalDays <= 60) // Example: Deadline is within the next 60 days
                            {
                                // Upcoming (Within 60 days)
                                UpcomingDeadlines.Add(doc);
                            }
                        }
                    }
                }
            }
        }
    }
}