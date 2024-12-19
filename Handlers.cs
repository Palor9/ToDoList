using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ToDoList.Server.Bodies;
using TrySQLite.DataBase;

namespace ToDoList.Handlers
{
    public class Handlers : Engine
    {
        public Handlers(string connectionString):
            base(connectionString) 
            { }

        public async Task HandleCreateTask(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request; //Запрос к серверу 
            
            using var reader = new StreamReader(request.InputStream, request.ContentEncoding); //Расшифровка ответа
            string data = await reader.ReadToEndAsync(); //Сам ответ

            var result = JsonSerializer.Deserialize<CreateTask>(data);
            Console.WriteLine($"name = {result.name}");

            var authId = request.Headers.Get("id");
            var modelAddId = Tools.ConverterNames(result);
            if (authId != null)
            {
                modelAddId.userID = Convert.ToInt32(authId);
            }
            else
            {
                context.Response.StatusCode = 400;
                context.Response.Close();
                return;
            }
            CreateTaskAsync(modelAddId);
            context.Response.StatusCode = 200;
            context.Response.Close();
        }

        public async Task HandleReadAllTasks(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response; //Ответ на запрос от сервера

            var authId = request.Headers.Get("id");
            
            if (authId == null)
            {
                context.Response.StatusCode = 400;
                context.Response.Close();
            }
            
            var infoRes = await ReadAllTasksAsync(Convert.ToInt32(authId)); //Чтение всех записей
            
            if (infoRes.Count != null)
            {
                List<ReadTask> infoResConverted = new List<ReadTask>();

                foreach (var task in infoRes)
                {
                    infoResConverted.Add(Tools.ConverterToReadTask(task));
                }

                byte[] buffer = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(infoResConverted));

                response.ContentLength64 = buffer.Length;//Может понадобиться клиенту (бесполезное дерьмо)
                response.ContentType = "application/json";

                using var outPut = response.OutputStream;

                await outPut.WriteAsync(buffer, 0, buffer.Length);
            }
        }

        public async Task HandlerUpdateTask(HttpListenerContext context)
        {
            HttpListenerResponse response = context.Response;
            HttpListenerRequest request = context.Request; //Запрос к серверу 

            var authId = request.Headers.Get("id");
            
            if (authId == null)
            {
                context.Response.StatusCode = 400;
                context.Response.Close();
            }
            
            using var reader = new StreamReader(request.InputStream, request.ContentEncoding); //Расшифровка ответа
            string data = await reader.ReadToEndAsync(); //Сам ответ

            var result = JsonSerializer.Deserialize<ReadTask>(data);

            await UpdateAsync(Tools.ConvertToTaskDB(result),Convert.ToInt32(authId));
            context.Response.StatusCode = 200;
            context.Response.Close();
        }

        public async Task HandlerReadTaskAsync(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request; //Запрос к серверу 
            HttpListenerResponse response = context.Response; //Ответ на запрос от сервера
            
            var authId = request.Headers.Get("id");
            
            if (authId == null)
            {
                context.Response.StatusCode = 400;
                context.Response.Close();
            }

            string paramValue = request.QueryString["id"];

            var taskDB = ReadTaskAsync(Convert.ToInt32(paramValue), Convert.ToInt32(authId));

            byte[] buffer = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(Tools.ConverterToReadTask(taskDB.Result)));

            using var outPut = response.OutputStream;
            await outPut.WriteAsync(buffer, 0, buffer.Length);
        }
        public async Task HandlerDeleteTask(HttpListenerContext context)
        {
            HttpListenerResponse response = context.Response;
            HttpListenerRequest request = context.Request; //Запрос к серверу 
            
            var authId = request.Headers.Get("id");
            
            if (authId == null)
            {
                context.Response.StatusCode = 400;
                context.Response.Close();
            }
            
            using var reader = new StreamReader(request.InputStream, request.ContentEncoding); //Расшифровка ответа
            string data = await reader.ReadToEndAsync(); //Сам ответ

            var result = JsonSerializer.Deserialize<ReadTask>(data);

            await DeleteAsync(result.id, Convert.ToInt32(authId));
            context.Response.StatusCode = 200;
            context.Response.Close();
        }

        public async Task HandlerCreateNewUser(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request; //Запрос к серверу 
            HttpListenerResponse response = context.Response; //Ответ на запрос от сервера
            using var reader = new StreamReader(request.InputStream, request.ContentEncoding); //Расшифровка ответа
            string data = await reader.ReadToEndAsync(); //Сам ответ

            var result = JsonSerializer.Deserialize<User>(data);

            await CreateUser(Tools.ConvertToUser(result));
            context.Response.StatusCode = 200;
            context.Response.Close();
        }
    }
}
