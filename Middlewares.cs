using System.Collections.Specialized;
using TrySQLite.DataBase;
using System.Net;
using System.Text;

namespace ToDoList;

public class Middlewares : Handlers.Handlers
{
    public Middlewares(string connectionString):
        base(connectionString) 
    { }

    public Func<HttpListenerContext, Task> Decorate(Func<HttpListenerContext, Task> next)
    {
        return (async (context) =>
        {
            var result = await RegisteredUnit(context.Request);
            if (result <= 0) //Проверка, есть ли пользователь в списке 
            {
                context.Response.StatusCode = 403; //Если нет - 403 и допуска нет
                context.Response.Close();
                return;
            }

            NameValueCollection query = new NameValueCollection();
            query["id"] = result.ToString();
            context.Request.Headers.Add(query);
            await next(context);
        });
    }
    
    private async Task <int> RegisteredUnit(HttpListenerRequest request)
    {
        string auth = request.Headers.Get("Authorization"); //Получение свзки данных о пользователе (логин\пасс)

        Console.WriteLine(auth);

        string[] authR = auth.Split(' '); //Разбивание связки на массив
        byte[] result = Convert.FromBase64String(authR[1]); //Приведение пасса к байтам
        string result1 = Encoding.Default.GetString(result); //Дешевровка данных
            
        Console.WriteLine(result1);

        string[] authedUser = result1.Split(':');//Разбивание связки на массив
        return await ReadLogAndPassAsync(authedUser[0], authedUser[1]);//Возвращение связки 
    }
}