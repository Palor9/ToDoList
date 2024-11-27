using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.dataBase.Models;
using ToDoList.Server.Bodies;
using System.Security.Cryptography;

namespace ToDoList
{
    internal class Tools //Инструменты ковертации классов
    {
        public static TaskDB ConverterNames(CreateTask info) //CreateTask в TaskDB
        {
            TaskDB taskDB = new TaskDB();
            taskDB.name = info.name;

            return taskDB;
        }

        public static ReadTask ConverterToReadTask(TaskDB info) //TaskDB в ReadTask
        {
            ReadTask readTask = new ReadTask();
            readTask.id = info.id;
            readTask.name = info.name;

            return readTask;
        }

        public static TaskDB ConvertToTaskDB(ReadTask readTask)
        {
            TaskDB taskDB = new TaskDB();
            taskDB.name = readTask.name;
            taskDB.id = readTask.id;

            return taskDB;
        }

        public static UserDB ConvertToUser(User user)
        {
            UserDB userDB = new UserDB();
            userDB.id = user.id;
            userDB.login = user.login;
            userDB.password = ComputeSha256Hash(user.password);

            return userDB;
        }
        public static string ComputeSha256Hash(string rawData)
        {
            // Создаем объект SHA256
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Вычисляем хэш для строки
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Преобразуем массив байт в строку
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
