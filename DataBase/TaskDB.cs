using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.dataBase.Models
{
    public class TaskDB  //Модель для БД
    {
        public int id;
        public string name;
        public int userID;
    }

    public class UserDB ///Модель для создания пользователя 
    {
        public int id;
        public string login;
        public string password;
    }
}
