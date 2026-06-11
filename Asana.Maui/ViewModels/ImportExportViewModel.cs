using Asana.Library.Models;
using Asana.Library.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;


namespace Asana.Maui.ViewModels
{
    public class ImportExportViewModel
    {
        public ICommand ExportCommand { get; }
        public ICommand ImportCommand { get; }

        public ImportExportViewModel()
        {
            ExportCommand = new Command(ExportData);
            ImportCommand = new Command(ImportData);
        }

        private string ExportFilePath => Path.Combine(AppContext.BaseDirectory, "ExportedData", "asana_export.json");


        public void ExportData()
        {
            try
            {
                var exportDir = Path.Combine(AppContext.BaseDirectory, "ExportedData");
                Directory.CreateDirectory(exportDir);

                var exportPath = Path.Combine(exportDir, "asana_export.json");
                var exportObj = new
                {
                    Projects = ProjectServiceProxy.Current.Projects,
                    ToDos = ToDoServiceProxy.Current.ToDos
                };
                var json = JsonSerializer.Serialize(exportObj, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(exportPath, json);

                //Show the path in the UI
                Application.Current?.MainPage?.DisplayAlert("Export", $"Data successfully exported to:\n{exportPath}", "OK");
            }
            catch (Exception ex)
            {
                Application.Current?.MainPage?.DisplayAlert("Export Error", ex.Message, "OK");
            }
        }

        public void ImportData()
        {
            

            try
            {
                if (!File.Exists(ExportFilePath))
                {
                    Application.Current?.MainPage?.DisplayAlert("Import Error", $"File not found:\n{ExportFilePath}", "OK");
                    return;
                }

                var json = File.ReadAllText(ExportFilePath);
                var importObj = JsonSerializer.Deserialize<ExportDataModel>(json);
                

                if (importObj != null)
                {
                    ProjectServiceProxy.Current.Projects.Clear();
                    ProjectServiceProxy.Current.Projects.AddRange(importObj.Projects);

                    ToDoServiceProxy.Current.ToDos.Clear();
                    ToDoServiceProxy.Current.ToDos.AddRange(importObj.ToDos);

                    //Re-link ToDos in Projects to the global ToDo list by Id
                    var toDoDict = ToDoServiceProxy.Current.ToDos.ToDictionary(t => t.Id);
                    foreach (var project in ProjectServiceProxy.Current.Projects)
                    {
                        project.ToDos = project.ToDos
                            .Select(t => toDoDict.TryGetValue(t.Id, out var globalToDo) ? globalToDo : t)
                            .ToList();
                    }

                    Application.Current?.MainPage?.DisplayAlert("Import", "Data successfully imported.", "OK");

                    //Show imported data for verification
                    Application.Current?.MainPage?.DisplayAlert("Projects", GetProjectsDebugString(), "OK");
                    Application.Current?.MainPage?.DisplayAlert("ToDos", GetToDosDebugString(), "OK");
                
                }
                else
                {
                    Application.Current?.MainPage?.DisplayAlert("Import Error", "File format is invalid.", "OK");
                }
            }
            catch (Exception ex)
            {
                Application.Current?.MainPage?.DisplayAlert("Import Error", ex.Message, "OK");
            }
        }

        //Helper class for deserializing the exported Json
        public class ExportDataModel
        {
            public List<Project> Projects { get; set; } = new();
            public List<ToDo> ToDos { get; set; } = new();
        }

        //Debug string helper methods
        public string GetProjectsDebugString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Projects:");
            foreach (var project in ProjectServiceProxy.Current.Projects)
            {
                sb.AppendLine($"Project {project.Id}: {project.Name} - {project.Description}");
                foreach (var todo in project.ToDos)
                {
                    sb.AppendLine($"  ToDo {todo.Id}: {todo.Name} (ProjectId: {todo.ProjectId})");
                }
            }
            return sb.ToString();
        }

        public string GetToDosDebugString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("ToDos:");
            foreach (var todo in ToDoServiceProxy.Current.ToDos)
            {
                sb.AppendLine($"ToDo {todo.Id}: {todo.Name} (ProjectId: {todo.ProjectId})");
            }
            return sb.ToString();
        }

    }
}