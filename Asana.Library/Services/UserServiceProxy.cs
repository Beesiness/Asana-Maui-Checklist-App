using Asana.Library.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Asana.Library.Services
{
    public class UserServiceProxy
    {
        private ObservableCollection<User> _userList;
        public ObservableCollection<User> Users
        {
            get => _userList;
            private set
            {
                if (value != _userList)
                    _userList = value;
            }
        }

        private static UserServiceProxy? instance;
        public static UserServiceProxy Current
        {
            get
            {
                if (instance == null)
                    instance = new UserServiceProxy();
                return instance;
            }
        }

        private UserServiceProxy()
        {

            var toDoService = ToDoServiceProxy.Current;
            //Roundabout way to ensure picker is updated because property setter is used
            //This roundabout fix is still not working but I CBA anymore

            var admin = new User { Id = 0 };
            admin.DisplayName = "Admin";
            admin.Username = "AdminUser";


            var user1 = new User { Id = 1 };
            user1.DisplayName = "User 1";
            user1.Username = "UserName1";
            user1.AssignedToDos = new ObservableCollection<ToDo>
            {
                toDoService.ToDos.FirstOrDefault(t => t.Id == 1),
                toDoService.ToDos.FirstOrDefault(t => t.Id == 2)
            };

            var user2 = new User { Id = 2 };
            user2.DisplayName = "User 2";
            user2.Username = "UserName2";

            var user3 = new User { Id = 3 };
            user3.DisplayName = "User 3";
            user3.Username = "UserName3";

            _userList = new ObservableCollection<User> { admin, user1, user2, user3 };
        }
    }
}

