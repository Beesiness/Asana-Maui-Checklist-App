using Asana.Library.Models;
using Asana.Library.Services;
using Asana.Maui.ViewModels;

namespace Asana.Maui.Views;

[QueryProperty(nameof(UserId), "userId")]
public partial class UserDetailView : ContentPage
{
    public int UserId { get; set; }


    public UserDetailView()
	{
		InitializeComponent();
	}

    private void CancelClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//ManageUsers");
    }

    private void ContentPage_NavigatedFrom(object sender, NavigatedFromEventArgs e)
    {

    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        if (UserId > 0)
        {
            //Editing existing user
            var user = UserServiceProxy.Current.Users.FirstOrDefault(u => u.Id == UserId);
            BindingContext = user != null ? new UserDetailViewModel(user) : new UserDetailViewModel();
        }
        else
        {
            //Adding new user
            BindingContext = new UserDetailViewModel();
        }
    }

}