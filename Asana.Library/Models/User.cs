using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Asana.Library.Models
{
    public class User : INotifyPropertyChanged
    {
        public int Id { get; set; }

        private string displayName = string.Empty;
        public string DisplayName
        {
            get => displayName;
            set
            {
                if (displayName != value)
                {
                    displayName = value;
                    OnPropertyChanged();
                }
            }
        }

        private string username = string.Empty;
        public string Username
        {
            get => username;
            set
            {
                if (username != value)
                {
                    username = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private ObservableCollection<ToDo> assignedToDos = new();
        public ObservableCollection<ToDo> AssignedToDos
        {
            get => assignedToDos;
            set
            {
                if (assignedToDos != value)
                {
                    assignedToDos = value;
                    OnPropertyChanged();
                }
            }
        }
        public IEnumerable<int> AssignedToDoIds => AssignedToDos.Select(t => t.Id);
    }

}
