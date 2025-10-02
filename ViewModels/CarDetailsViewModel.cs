using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using CarDeadlineTracker.Data;
using CarDeadlineTracker.Model;
using CarDeadlineTracker.Views;
using Microsoft.EntityFrameworkCore;

namespace CarDeadlineTracker.ViewModels;

public class CarDetailsViewModel : ViewModelBase
{
    // The main car object being displayed
    public Car SelectedCar { get; set; }
    private RenewalItem _selectedRenewalItem;
    public RenewalItem SelectedRenewalItem
    {
        get => _selectedRenewalItem;
        set
        {
            _selectedRenewalItem =value;
            OnPropertyChanged();
        }
    }
    
    private RepairLog _selectedRepairLog;

    public RepairLog SelectedRepairLog
    {
        get => _selectedRepairLog;
        set
        {
            _selectedRepairLog =value;
            OnPropertyChanged();
        }
    }
    // Observable collections for the nested data
    public ObservableCollection<RenewalItem> RenewalItems { get; set; } = new ObservableCollection<RenewalItem>();
    public ObservableCollection<RepairLog> RepairLogs { get; set; } = new ObservableCollection<RepairLog>();

    private List<RenewalItem> _renewalItems => RenewalItems.ToList();
    private List<RepairLog> _repairLogs => RepairLogs.ToList();
    
    // Commands for managing the data
    public ICommand ToggleDoneCommand { get; }
    public ICommand AddRenewalItemCommand { get; }
    public ICommand EditRenewalItemCommand { get; }
    public  ICommand DeleteRenewalItemCommand { get; }
    public ICommand AddRepairCommand { get; }
    public ICommand EditRepairCommand { get; }
    public ICommand DeleteRepairCommand { get; }
    
    public ICommand GenerateSummaryCommand { get; }

    

    // Constructor to load data for a specific car
    public CarDetailsViewModel(Car car)
    {
        SelectedCar = car;
        GenerateSummaryCommand = new RelayCommand(OpenSummaryWindow);
        LoadCarDetails();
        AddRenewalItemCommand = new RelayCommand(AddRenewalItem);
        EditRenewalItemCommand = new RelayCommand(EditRenewalItem,CanEditRenewalItem);
        DeleteRenewalItemCommand = new RelayCommand(DeleteRenewalItem, CanEditRenewalItem);
        AddRepairCommand = new RelayCommand(AddRepairLog);
        EditRepairCommand = new RelayCommand(EditRepairLog, CanEditRepair);
        DeleteRepairCommand = new RelayCommand(DeleteRepairLog, CanEditRepair);
        ToggleDoneCommand = new RelayCommand(ToggleDocumentDone);
    }

    private void OpenSummaryWindow(object parameter)
    {
        var ViewModel = new GenerateSummaryViewModel(SelectedCar,_renewalItems,_repairLogs);
        var View = new GenerateSummaryView();
        
        View.DataContext = ViewModel;
        View.ShowDialog();
    }

    private bool CanEditRenewalItem(object parameter)
    {
        return SelectedRenewalItem != null;
    }

    private bool CanEditRepair(object parameter)
    {
        return SelectedRepairLog != null;
    }

    

    private void LoadCarDetails()
    {
        using (var dbContext = new ApplicationDbContext())
        {
            // Use .Include() to eagerly load the related data
            var carWithDetails = dbContext.Cars
                .Include(c => c.RenewalItems)
                .Include(c => c.RepairLogs)
                .FirstOrDefault(c => c.NumberPlate == SelectedCar.NumberPlate);

            if (carWithDetails != null)
            {
                RenewalItems.Clear();
                foreach (var doc in carWithDetails.RenewalItems)
                {
                    RenewalItems.Add(doc);
                }

                RepairLogs.Clear();
                foreach (var repair in carWithDetails.RepairLogs)
                {
                    RepairLogs.Add(repair);
                }
            }
            
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
    
    // Stub methods for the commands
    private void AddRenewalItem(object parameter)
    {
        var addRenewalItemWindow = new AddEditRenewalItemView();
    
        // Pass the car's number plate to the new view model
        var viewModel = new AddEditRenewalItemViewModel(SelectedCar.NumberPlate);
    
        addRenewalItemWindow.DataContext = viewModel;
    
        addRenewalItemWindow.ShowDialog();
    
        // Reload the car details to display the newly added document
        LoadCarDetails();
    }
    
    private void EditRenewalItem(object parameter)
    {
        if(SelectedRenewalItem == null) return;
        var editRenewalItemWindow = new AddEditRenewalItemView();
        var viewModel = new AddEditRenewalItemViewModel(SelectedCar.NumberPlate,SelectedRenewalItem);
        editRenewalItemWindow.DataContext = viewModel;
        editRenewalItemWindow.ShowDialog();
        LoadCarDetails();
    }

    private void DeleteRenewalItem(object parameter)
    {
        if(SelectedRenewalItem == null) return;
        var result = MessageBox.Show($"Are you sure you want to delete this {SelectedRenewalItem}?", 
            "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        if (result == MessageBoxResult.Yes)
        {
            using (var dbContext = new ApplicationDbContext())
            {
                // The key part of the DELETE operation
                dbContext.RenewalItems.Remove(SelectedRenewalItem);
                dbContext.SaveChanges();
            }
            // Refresh the UI list
            LoadCarDetails();
        }
    }
    
    
    private void AddRepairLog(object parameter) 
    {
        var addRepairWindow = new AddEditRepairView();
                       
        // Pass the car's number plate to the new view model
        var viewModel = new AddEditRepairViewModel(SelectedCar.NumberPlate);
    
        addRepairWindow.DataContext = viewModel;
    
        addRepairWindow.ShowDialog();
    
        // Reload the car details to display the newly added document
        LoadCarDetails();
    }
    private void EditRepairLog(object parameter)
    {
        if(SelectedRepairLog == null) return;
        var editRepairLogWindow = new AddEditRepairView();
        var viewModel = new AddEditRepairViewModel(SelectedCar.NumberPlate,SelectedRepairLog);
        editRepairLogWindow.DataContext = viewModel;
        editRepairLogWindow.ShowDialog();
        LoadCarDetails();
    }

    private void DeleteRepairLog(object parameter)
    {
        if(SelectedRepairLog == null) return;
        var result = MessageBox.Show($"Are you sure you want to delete this {SelectedRepairLog}?", 
            "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        if (result == MessageBoxResult.Yes)
        {
            using (var dbContext = new ApplicationDbContext())
            {
                // The key part of the DELETE operation
                dbContext.RepairLogs.Remove(SelectedRepairLog);
                dbContext.SaveChanges();
            }
            // Refresh the UI list
            LoadCarDetails();
        }
    }
}