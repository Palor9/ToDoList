using System.Net;

namespace ToDoList.Server
{
    public class Server: Middlewares
    {
        HttpListener listener; //Чтение запросов
        Dictionary<string, Func<HttpListenerContext, Task>> router = new Dictionary<string, Func<HttpListenerContext, Task>>(); //Маршрутизатор для обработки запроса 

        public Server(string host, string connectionString):
            base(connectionString)
        {
            listener = new HttpListener();
            listener.Prefixes.Add(host);
            router.Add("GET /task", Decorate(HandleReadAllTasks)); //Выбор метода для обработки запроса чтения
            router.Add("GET /task/id", Decorate(HandlerReadTaskAsync)); //Выбор метода для обработки запроса чтения
            router.Add("POST /task", Decorate(HandleCreateTask)); //Выбор метода для обработки запроса создания
            router.Add("PATCH /task", Decorate(HandlerUpdateTask)); //Выбор метода для обработки запроса обновления данных
            router.Add("DELETE /task", Decorate(HandlerDeleteTask)); //Выбор метода для удаления данных
            router.Add("POST /user", HandlerCreateNewUser); //Выбор метода для создания пользователя
        }

        public async Task Start()
        {
            listener.Start();
            Console.WriteLine("Server starts listening");
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
