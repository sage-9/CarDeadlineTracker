using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;
using CarDeadlineTracker.Data;
using CarDeadlineTracker.Model;
using CarDeadlineTracker.ViewModels;
using CarDeadlineTracker.Views;

public class CarDetailsViewModel : ViewModelBase
{
    // The main car object being displayed
    public Car SelectedCar { get; set; }

    // Observable collections for the nested data
    public ObservableCollection<Document> Documents { get; set; } = new ObservableCollection<Document>();
    public ObservableCollection<MaintenanceRecord> MaintenanceRecords { get; set; } = new ObservableCollection<MaintenanceRecord>();
    public ObservableCollection<RepairLog> RepairLogs { get; set; } = new ObservableCollection<RepairLog>();
    
    // Commands for managing the data
    public ICommand AddDocumentCommand { get; }
    public ICommand AddMaintenanceCommand { get; }
    public ICommand AddRepairCommand { get; }

    // Constructor to load data for a specific car
    public CarDetailsViewModel(Car car)
    {
        SelectedCar = car;
        LoadCarDetails();

        AddDocumentCommand = new RelayCommand(AddDocument);
        AddMaintenanceCommand = new RelayCommand(AddMaintenance);
        AddRepairCommand = new RelayCommand(AddRepair);
    }

    private void LoadCarDetails()
    {
        using (var dbContext = new ApplicationDbContext())
        {
            // Use .Include() to eagerly load the related data
            var carWithDetails = dbContext.Cars
                .Include(c => c.Documents)
                .Include(c => c.MaintenanceRecords)
                .Include(c => c.RepairLogs)
                .FirstOrDefault(c => c.NumberPlate == SelectedCar.NumberPlate);

            if (carWithDetails != null)
            {
                Documents.Clear();
                foreach (var doc in carWithDetails.Documents)
                {
                    Documents.Add(doc);
                }

                MaintenanceRecords.Clear();
                foreach (var maint in carWithDetails.MaintenanceRecords)
                {
                    MaintenanceRecords.Add(maint);
                }

                RepairLogs.Clear();
                foreach (var repair in carWithDetails.RepairLogs)
                {
                    RepairLogs.Add(repair);
                }
            }
        }
    }
    
    // Stub methods for the commands
    private void AddDocument(object parameter)
    {
        var addDocumentWindow = new AddEditDocumentView();
    
        // Pass the car's number plate to the new view model
        var viewModel = new AddEditDocumentViewModel(SelectedCar.NumberPlate);
    
        addDocumentWindow.DataContext = viewModel;
    
        addDocumentWindow.ShowDialog();
    
        // Reload the car details to display the newly added document
        LoadCarDetails();
    }
    private void AddMaintenance(object parameter)
    {
        var addMaintenanceWindow = new AddEditMaintenanceView();
    
        // Pass the car's number plate to the new view model
        var viewModel = new AddEditMaintenanceViewModel(SelectedCar.NumberPlate);
    
        addMaintenanceWindow.DataContext = viewModel;
    
        addMaintenanceWindow.ShowDialog();
    
        // Reload the car details to display the newly added document
        LoadCarDetails();
    }
    private void AddRepair(object parameter) 
    {
        var addRepairWindow = new AddEditRepairView();
    
        // Pass the car's number plate to the new view model
        var viewModel = new AddEditRepairViewModel(SelectedCar.NumberPlate);
    
        addRepairWindow.DataContext = viewModel;
    
        addRepairWindow.ShowDialog();
    
        // Reload the car details to display the newly added document
        LoadCarDetails();
    }
}