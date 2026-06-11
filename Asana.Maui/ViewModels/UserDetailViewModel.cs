using Asana.Library.Models;
using Asana.Library.Services;
using Microsoft.Maui;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Asana.Maui.ViewModels
{
    public class UserDetailViewModel
    {
        public User Model { get; set; }
        public ObservableCollection<ToDo> AllToDos { get; }
        public ICommand AssignToDoCommand { get; }
        public bool IsNew { get; set; }

        private ToDo? selectedToDo;
        public ToDo? SelectedToDo
        {
            get => selectedToDo;
            set
            {
                if (selectedToDo != value)
                {
                    selectedToDo = value;
                }
            }
        }

        public UserDetailViewModel()
        {
            Model = new User();
            IsNew = true;
            AllToDos = new ObservableCollection<ToDo>(ToDoServiceProxy.Current.ToDos);
            AssignToDoCommand = new Command(AssignToDo);
        }

        public UserDetailViewModel(User user)
        {
            Model = user;
            IsNew = false;
            AllToDos = new ObservableCollection<ToDo>(ToDoServiceProxy.Current.ToDos);
            AssignToDoCommand = new Command(AssignToDo);
        }

        private async void AssignToDo()
        {
            //Handle the case where no ToDo is selected
            if (SelectedToDo == null)
            {
                //If the user is new, add them to the UserServiceProxy
                if (IsNew)
                {
                    Model.Id = UserServiceProxy.Current.Users.Any()
                        ? UserServiceProxy.Current.Users.Max(u => u.Id) + 1
                        : 1;

                    UserServiceProxy.Current.Users.Add(Model);
                    await Application.Current.MainPage.DisplayAlert("Success", $"User '{Model.DisplayName}' has been created.", "OK");
                }
                else
                {
                    //Update the existing user's properties
                    var userInProxy = UserServiceProxy.Current.Users.FirstOrDefault(u => u.Id == Model.Id);
                    if (userInProxy != null)
                    {
                        userInProxy.DisplayName = Model.DisplayName;
                        userInProxy.Username = Model.Username;
                        await Application.Current.MainPage.DisplayAlert("Success", $"User '{Model.DisplayName}' has been updated.", "OK");
                    }
                }

                //Navigate back to the Manage Users page
                await Shell.Current.GoToAsync("//ManageUsers");
                return;
            }

            //Handle the case where a ToDo is selected
            if (!Model.AssignedToDos.Any(t => t.Id == SelectedToDo.Id))
            {
                Model.AssignedToDos.Add(SelectedToDo);

                var userInProxy = UserServiceProxy.Current.Users.FirstOrDefault(u => u.Id == Model.Id);
                if (userInProxy != null)
                {
                    userInProxy.AssignedToDos.Add(SelectedToDo);
                }

                await Application.Current.MainPage.DisplayAlert("Success", $"ToDo '{SelectedToDo.Name}' assigned to {Model.DisplayName}.", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Info", $"ToDo '{SelectedToDo.Name}' is already assigned to {Model.DisplayName}.", "OK");
            }

            //Navigate back to the Manage Users page
            await Shell.Current.GoToAsync("//ManageUsers");
        }
    }
    
}
