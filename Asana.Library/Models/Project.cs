using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asana.Library.Models
{
    public class Project
    {
        public Project()
        {
            Id = 0;
            Name = string.Empty;
            Description = string.Empty;
            ToDos = new List<ToDo>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //CompletePercent property calculates the percentage of completed ToDos in the project
        public string CompletePercent
        {
            get
            {
                if (ToDos.Count == 0) return "0%";
                var completedCount = ToDos.Count(t => t.IsCompleted == true);
                return $"{(completedCount * 100 / ToDos.Count)}%";
            }
        }
        public List<ToDo> ToDos { get; set; }
        
    }
}
