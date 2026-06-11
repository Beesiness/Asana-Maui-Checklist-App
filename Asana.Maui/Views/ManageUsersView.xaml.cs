using Asana.Maui.ViewModels;

namespace Asana.Maui.Views;

public partial class ManageUsersView : ContentPage
{
	public ManageUsersView()
	{
		InitializeComponent();
	}

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
        BindingContext = new ManageUsersViewModel();
    }
}