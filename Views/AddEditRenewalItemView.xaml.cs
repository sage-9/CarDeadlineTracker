using System.Windows;
using System.Windows.Input;

namespace CarDeadlineTracker.Views;

public partial class AddEditRenewalItemView : Window
{
    public AddEditRenewalItemView()
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