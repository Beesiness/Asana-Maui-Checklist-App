using Asana.Maui.ViewModels;

namespace Asana.Maui
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel();
        }

        private void AddNewClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//ToDoDetails");
        }

        private void AddNewProjectClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//ProjectDetails");
        }

        private void EditClicked(object sender, EventArgs e)
        {
            var selectedId = (BindingContext as MainPageViewModel)?.SelectedToDoId ?? 0;
            Shell.Current.GoToAsync($"//ToDoDetails?toDoId={selectedId}");
        }

        private void UpdateProjectClicked(object sender, EventArgs e)
        {
            var selectedId = (BindingContext as MainPageViewModel)?.SelectedProjectId ?? 0;
            Shell.Current.GoToAsync($"//ProjectDetails?ProjectId={selectedId}");
        }

        private void DeleteClicked(object sender, EventArgs e)
        {
            (BindingContext as MainPageViewModel)?.DeleteToDo();
        }

        private void DeleteProjectClicked(object sender, EventArgs e)
        {
            (BindingContext as MainPageViewModel)?.DeleteProject();
        }

        private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
        {
            //BindingContext = new MainPageViewModel();   //Tried this for reinstantiating and therefore getting data from import
            (BindingContext as MainPageViewModel)?.RefreshPage();
        }

        private void ContentPage_NavigatedFrom(object sender, NavigatedFromEventArgs e)
        {

        }

        private void InLineDeleteClicked(object sender, EventArgs e)
        {
            //This was class' implementation but I couldn't get it to work with the Project InLineDelete
            //(BindingContext as MainPageViewModel)?.RefreshPage();
            if (sender is Button button)
            {
                if (button.BindingContext is ProjectDetailViewModel projectVm)
                {
                    projectVm.DoDelete();
                    (BindingContext as MainPageViewModel)?.RefreshPage();
                }
                else if (button.BindingContext is ToDoDetailViewModel toDoVm)
                {
                    toDoVm.DoDelete();
                    (BindingContext as MainPageViewModel)?.RefreshPage();
                }
            }
        }

        private void RemoveFromProjectClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is ToDoDetailViewModel toDoVm)
            {
                toDoVm.RemoveFromProject();
                (BindingContext as MainPageViewModel)?.RefreshPage();
            }
        }

        private void SearchClicked(object sender, EventArgs e)
        {
        
            (BindingContext as MainPageViewModel)?.HandleSearchClicked();

        }

        private void ImportExportClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//ImportExport");
        }

        private void ManageUsersClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//ManageUsers");
        }

        
    }

}
