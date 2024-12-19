using ToDoList.Server;

namespace TrySQLite
{
    public class EntryPoint //Подключение к серверу
    {
        const string connectionString = "Host=localhost;Username=user;Password=1209;Database=user"; //Адрес базы данных
        const string hostID = "http://localhost:8080/"; //Адрес хоста

        public static async Task Main()
        {
            Server server = new Server(hostID, connectionString);
            await server.Start();                                 //Запуск сервера
        }

        //public static async Task SimpleHandler(HttpListenerContext context)
        //{

        //    Dictionary<string, Func<HttpListenerContext, Task>> router = new Dictionary<string, Func<HttpListenerContext, Task>>
        //    {

        //    { "GET /", HandleCalc},
        //    { "POST /", HandleGreed}

        //    };

        //    string methodPath = $"{context.Request.HttpMethod} {context.Request.Url.AbsolutePath}";
        //    Console.WriteLine(methodPath);
        //    if (!router.ContainsKey(methodPath))
        //    {
        //        context.Response.StatusCode = 404;
        //        context.Response.Close();
        //        return;
        //    }

        //    var handler = router[methodPath];
        //    await handler(context);
        //}
    }
}