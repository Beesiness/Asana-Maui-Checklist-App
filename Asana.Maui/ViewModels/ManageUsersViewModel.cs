using Asana.Library.Models;
using Asana.Library.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;


namespace Asana.Maui.ViewModels
{
    public class ManageUsersViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<User> Users { get; }
        private User selectedUser;
        public User SelectedUser
        {
            get => selectedUser;
            set
            {
                if (selectedUser != value)
                {
                    selectedUser = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ICommand AddUserCommand { get; }
        public ICommand DeleteUserCommand { get; }
        public ICommand EditUserCommand { get; }

        public ManageUsersViewModel()
        {
            Users = UserServiceProxy.Current.Users;
            SelectedUser = null;
            AddUserCommand = new Command(() => Shell.Current.GoToAsync("//UserDetail"));
            EditUserCommand = new Command(() =>
            {
                if (SelectedUser != null)
                {
                    Shell.Current.GoToAsync($"//UserDetail?userId={SelectedUser.Id}");
                }
            });
            DeleteUserCommand = new Command(DeleteUser);
        }


        private void DeleteUser()
        {
            if (SelectedUser != null)
            {
                Users.Remove(SelectedUser);
                UserServiceProxy.Current.Users.Remove(SelectedUser);
                SelectedUser = null;
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
    
        