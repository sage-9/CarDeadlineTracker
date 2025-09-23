using System.Windows;
using System.Windows.Input;
using CarDeadlineTracker.Data;
using CarDeadlineTracker.Model;
using CarDeadlineTracker.ViewModels;

public class AddEditDocumentViewModel : ViewModelBase
{
    public Document Document { get; set; } = new Document();
    public string CarNumberPlate { get; set; }

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public AddEditDocumentViewModel(string carNumberPlate)
    {
        CarNumberPlate = carNumberPlate;
        SaveCommand = new RelayCommand(SaveDocumentAndClose);
        CancelCommand = new RelayCommand(CancelAndClose);
    }

    private void SaveDocumentAndClose(object parameter)
    {
        // Link the new document to the correct car
        Document.CarNumberPlate = CarNumberPlate;
        Document.Notes = Document.Notes ?? string.Empty;

        using (var dbContext = new ApplicationDbContext())
        {
            dbContext.Documents.Add(Document);
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