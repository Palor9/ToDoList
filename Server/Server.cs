using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TrySQLite.DataBase;
using static TrySQLite.EntryPoint;
using ToDoList.Server.Bodies;
using ToDoList.Handlers;


namespace ToDoList.Server
{
    public class Server: Handlers.Handlers
    {
        HttpListener listener; //Чтение запросов
        Dictionary<string, Func<HttpListenerContext, Task>> router = new Dictionary<string, Func<HttpListenerContext, Task>>(); //Маршрутизатор для обработки запроса 

        public Server(string host, string connectionString):
            base(connectionString)
        {
            listener = new HttpListener();
            listener.Prefixes.Add(host);
            router.Add("GET /task", HandleReadAllTasks); //Выбор метода для обработки запроса чтения
            router.Add("GET /task/id", HandlerReadTaskAsync); //Выбор метода для обработки запроса чтения
            router.Add("POST /task", HandleCreateTask); //Выбор метода для обработки запроса создания
            router.Add("PATCH /task", HandlerUpdateTask); //Выбор метода для обработки запроса обновления данных
            router.Add("DELETE /task", HandlerDeleteTask); //Выбор метода для удаления данных
            router.Add("POST /user", HandlerCreateNewUser); //Выбор метода для создания пользователя
        }

        public async Task Start()
        {
            listener.Start();

            while (true)
            {
                HttpListenerContext context = await listener.GetContextAsync();//Какой запрос пришел и ответ соответствующий
                string methodPath = $"{context.Request.HttpMethod} {context.Request.Url.AbsolutePath}";//Какой метод и куда он был отправлен 
                Console.WriteLine(methodPath);
                if (!router.ContainsKey(methodPath))
                {
                    context.Response.StatusCode = 404;
                    context.Response.Close();
                    return;
                }

                var handler = router[methodPath];//Подстановка обработчика для запроса 
                await handler(context);//Выполнение запроса
            }
        }
    }
}
