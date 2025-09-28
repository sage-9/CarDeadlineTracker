using System.Windows;
using System.Windows.Input;

namespace CarDeadlineTracker.Views;

public partial class AddEditRepairView : Window
{
    public AddEditRepairView()
    {
        InitializeComponent();
    }
    
    private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        if (!e.Text.All(char.IsDigit))
        {
            e.Handled = true;
        }
        
    }
}