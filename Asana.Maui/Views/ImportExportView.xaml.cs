using Asana.Library.Models;
using Asana.Maui.ViewModels;

namespace Asana.Maui.Views;

public partial class ImportExportView : ContentPage
{
    public ImportExportView()
    {
        InitializeComponent();

    }

    /*private void ExportClicked(object sender, EventArgs e)
    {
        (BindingContext as ImportExportViewModel)?.ExportData();
        DisplayAlert("Export", "Data successfully exported.", "OK");
    }

    private void ImportClicked(object sender, EventArgs e)
    {
        (BindingContext as ImportExportViewModel)?.ImportData();

        //Find the MainPage and refresh its viewmodel
        foreach (var page in Shell.Current.Navigation.NavigationStack)
        {
            if (page is MainPage mainPage)
            {
                (mainPage.BindingContext as MainPageViewModel)?.RefreshPage();
                break;
            }
        }
    }*/

    private void CancelClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//MainPage");
    }

    private void OkClicked(object sender, EventArgs e)
    {
        
        Shell.Current.GoToAsync("//MainPage");
    }

    private void ContentPage_NavigatedFrom(object sender, NavigatedFromEventArgs e)
    {

    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        BindingContext = new ImportExportViewModel();
    }
}