using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ToDoList.Server.Bodies
{
    public class CreateTask //Модель для json
    {
        [JsonPropertyName("name")]
        public string name { get; set; }
    }
    public class ReadTask //Модель для json
    {
        [JsonPropertyName("id")]
        public int id { get; set; }

        [JsonPropertyName("name")]
        public string name { get; set; }
    }

    public class User
    {
        [JsonPropertyName("id")]
        public int id { get; set; }

        [JsonPropertyName("login")]
        public string login { get; set; }

        [JsonPropertyName("password")]
        public string password { get; set; }
    }
}
