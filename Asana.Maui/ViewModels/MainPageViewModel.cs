using Asana.Library.Models;
using Asana.Library.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Asana.Maui.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private ToDoServiceProxy _toDoSvc;
        private ProjectServiceProxy _projectSvc;

        public MainPageViewModel()
        {
            _toDoSvc = ToDoServiceProxy.Current;
            _projectSvc = ProjectServiceProxy.Current;
            Query = string.Empty;
            SelectedUser = Users.FirstOrDefault();
        }

        public ToDoDetailViewModel SelectedToDo { get; set; }

        private string query;
        public string Query
        {
            get { return query; }
            set
            {
                if (query!=value)
                {
                    query = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ProjectDetailViewModel SelectedProject { get; set; }
        public ObservableCollection<ToDoDetailViewModel> ToDos
        {
            get
            {
                //Start with all ToDos
                IEnumerable<ToDo> toDos = _toDoSvc.ToDos;

                //Filter #1: Only ToDos assigned to the selected user
                if (SelectedUser != null && SelectedUser.Id != 0) // "No User" has Id = 0
                {
                    toDos = SelectedUser.AssignedToDos;
                }

                //Filter #2: Search query
                if (!string.IsNullOrWhiteSpace(Query))
                {
                    toDos = toDos.Where(t =>
                        (t?.Name?.Contains(Query, StringComparison.OrdinalIgnoreCase) ?? false) ||
                        (t?.Description?.Contains(Query, StringComparison.OrdinalIgnoreCase) ?? false));
                }

                //Filter #3: Show completed
                if (!IsShowCompleted)
                {
                    toDos = toDos.Where(t => !(t?.IsCompleted ?? false));
                }

                //Sorting logic
                var toDoViewModels = toDos.Select(t => new ToDoDetailViewModel(t));
                toDoViewModels = selectedSortOption switch
                {
                    "Name" => toDoViewModels.OrderBy(t => t.Model.Name),
                    "Due Date" => toDoViewModels.OrderBy(t => t.Model.DueDate),
                    "Priority" => toDoViewModels.OrderBy(t => t.Model.Priority),
                    "Completed" => toDoViewModels.OrderBy(t => t.Model.IsCompleted),
                    _ => toDoViewModels
                };

                return new ObservableCollection<ToDoDetailViewModel>(toDoViewModels);
            }
        }

        public ObservableCollection<ProjectDetailViewModel> Projects
        {
            get
            {
                var projects = ProjectServiceProxy.Current.Projects
                        .Where(p => p.Name.Contains(Query) || p.Description.Contains(Query))
                        .Select(p => new ProjectDetailViewModel(p));

                projects = selectedSortOption switch
                {
                    "Project Name" => projects.OrderBy(p => p.Model?.Name),
                    _ => projects
                };


                return new ObservableCollection<ProjectDetailViewModel>(projects);
            }
        }


        public int SelectedToDoId => SelectedToDo?.Model?.Id ?? 0;
        public int SelectedProjectId => SelectedProject?.Model?.Id ?? 0;

        private bool isShowCompleted;
        public bool IsShowCompleted { 
            get
            {
                return isShowCompleted;
            }

            set
            {
                if (isShowCompleted != value)
                {
                    isShowCompleted = value;
                    NotifyPropertyChanged(nameof(ToDos));
                }
            }
        }

        public void DeleteToDo()
        {
            if (SelectedToDo == null)
            {
                return;
            }

            ToDoServiceProxy.Current.DeleteToDo(SelectedToDo.Model);
            NotifyPropertyChanged(nameof(ToDos));
        }

        public void DeleteProject()
        {
            if (SelectedProject == null)
            {
                return;
            }
            ProjectServiceProxy.Current.DeleteProject(SelectedProject.Model);
            NotifyPropertyChanged(nameof(Projects));
        }



        public void RefreshPage()
        {
            NotifyPropertyChanged(nameof(ToDos));
            NotifyPropertyChanged(nameof(Projects));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void HandleSearchClicked()
        {
            RefreshPage();
            Query = string.Empty;
        }

        public List<string> SortOptions { get; } = new List<string>
        {
            "Name",
            "Due Date",
            "Priority",
            "Completed",
            "Project Name"
        };

        private string selectedSortOption;
        public string SelectedSortOption
        {
            get => selectedSortOption;
            set
            {
                if (selectedSortOption != value)
                {
                    selectedSortOption = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(ToDos));
                    NotifyPropertyChanged(nameof(Projects));
                }
            }
        }

        public ObservableCollection<User> Users => UserServiceProxy.Current.Users;

        private User? selectedUser;
        public User? SelectedUser
        {
            get => selectedUser;
            set
            {
                if (selectedUser != value)
                {
                    if (selectedUser != null)
                    {
                        
                        selectedUser.AssignedToDos.CollectionChanged -= AssignedToDos_CollectionChanged;
                    }

                    selectedUser = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(CurrentUserDisplay));

                    if (selectedUser != null)
                    {
                        
                        selectedUser.AssignedToDos.CollectionChanged += AssignedToDos_CollectionChanged;
                    }

                    //Refresh ToDos when the selected user changes
                    NotifyPropertyChanged(nameof(ToDos));
                }
            }
        }

        private void AssignedToDos_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //Refreshing ToDos when the AssignedToDos changes
            NotifyPropertyChanged(nameof(ToDos));
        }

        public string CurrentUserDisplay => SelectedUser != null
        ? $"[{SelectedUser.Id}] {SelectedUser.DisplayName}    -    {SelectedUser.Username}"
        : "No user selected";

        

    }
}
