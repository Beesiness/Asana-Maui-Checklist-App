using Asana.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asana.Library.Services
{
    public class ProjectServiceProxy
    {
        private List<Project>? _projectList;
        private List<ToDo> _toDoList;
        /*public List<Project> Projects
        {
            get
            {
                return _projectList.Take(100).ToList();
            }

            private set
            {
                if (value != _projectList)
                {
                    _projectList = value;
                }
            }
        }*/

        public List<Project> Projects
        {
            get => _projectList;
            private set
            {
                if (value != _projectList)
                {
                    _projectList = value;
                }
            }
        }


        private ProjectServiceProxy()
        {
            _toDoList = ToDoServiceProxy.Current.ToDos;
            _projectList = new List<Project>
            {
                new Project{Id = 1, Name = "Project 1", Description = "My Project 1", 
                    ToDos =_toDoList.Where(t => t.ProjectId == 1).ToList()},
                new Project{Id = 2, Name = "Project 2", Description = "My Project 2", ToDos = new List<ToDo>()},
                new Project{Id = 3, Name = "Project 3", Description = "My Project 3", ToDos = new List<ToDo>()},
                new Project{Id = 4, Name = "Project 4", Description = "My Project 4", ToDos = new List<ToDo>()},
            };
        }

        private static ProjectServiceProxy? instance;

        private int nextKey
        {
            get
            {
                if (Projects.Any())
                {
                    return Projects.Select(t => t.Id).Max() + 1;
                }
                return 1;
            }
        }

        public static ProjectServiceProxy Current
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProjectServiceProxy();
                }

                return instance;
            }
        }
        public Project? AddOrUpdate(Project? project)
        {
            if (project != null && project.Id == 0)
            {
                project.Id = nextKey;
                _projectList.Add(project);
            }

            return project;
        }

        public void DisplayProjects(bool isShowCompleted = false)
        {
            if (isShowCompleted)
            {
                Projects.ForEach(Console.WriteLine);
            }
            /*else
            {
                Projects.Where(t => (t != null) || (t?.CompletePercent ?? "0"))
                                .ToList()
                                .ForEach(Console.WriteLine);
            }*/
        }

        public Project? GetById(int id)
        {
            return Projects.FirstOrDefault(t => t.Id == id);
        }

        public void DeleteProject(Project? project)
        {
            if (project == null)
            {
                return;
            }
            _projectList.Remove(project);
        }


        //This is used for debugging the data stored when importing new data
        public void LogProjects()
        {
            Console.WriteLine("Projects:");
            foreach (var project in Projects)
            {
                Console.WriteLine($"Project {project.Id}: {project.Name} - {project.Description}");
                foreach (var todo in project.ToDos)
                {
                    Console.WriteLine($"  ToDo {todo.Id}: {todo.Name} (ProjectId: {todo.ProjectId})");
                }
            }
        }

    }
}
