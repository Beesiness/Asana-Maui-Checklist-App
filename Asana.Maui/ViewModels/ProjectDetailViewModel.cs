using Asana.Library.Models;
using Asana.Library.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Asana.Maui.ViewModels
{
    public class ProjectDetailViewModel
    {
        public ProjectDetailViewModel() {
            Model = new Project();
            selectedToDo = null;
            DeleteCommand = new Command(DoDelete);
        }

        public ProjectDetailViewModel(int id)
        {
            Model = ProjectServiceProxy.Current.GetById(id) ?? new Project();

            DeleteCommand = new Command(DoDelete);
        }

        public ProjectDetailViewModel(Project? model)
        {
            Model = model ?? new Project();
            DeleteCommand = new Command(DoDelete);
        }

        public void DoDelete() {
            ProjectServiceProxy.Current.DeleteProject(Model);
        }

        public Project? Model { get ; set; }
        public int ProjectId {get; set; }
        public ICommand? DeleteCommand { get; set; }

        
        public void AddOrUpdateProject()
        {
            ProjectServiceProxy.Current.AddOrUpdate(Model);
        }

        public List<ToDo> AvailableToDos
        {
            get
            {
                if(Model == null)
                    return new List<ToDo>();
                //Only getting ToDos NOT already assigned to this project
                return ToDoServiceProxy.Current.ToDos
                    .Where(t => t.ProjectId != Model.Id)
                    .ToList();
            }
        }

        private ToDo? selectedToDo;
        public ToDo SelectedToDo
        {
            get => selectedToDo;
            set
            {
                if (value != null && Model != null)
                {
                    value.ProjectId = Model.Id;
                    if (!Model.ToDos.Contains(value))
                        Model.ToDos.Add(value);

                    //Update the ToDo in the service proxy
                    ToDoServiceProxy.Current.AddOrUpdate(value);
                    

                }
                selectedToDo = null; 
            }
        }

        //This exposes ToDos so that the nested To-Do List can use the ViewModel as it's source
        public IEnumerable<ToDoDetailViewModel> ToDoViewModels
        {
            get
            {
                if (Model?.ToDos == null)
                    return Enumerable.Empty<ToDoDetailViewModel>();
                return Model.ToDos.Select(t => new ToDoDetailViewModel(t));
            }
        }



    }
}
