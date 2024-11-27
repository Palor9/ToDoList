using Microsoft.Data.Sqlite;
using ToDoList;
using ToDoList.dataBase.Models;
using Npgsql;

namespace TrySQLite.DataBase
{
    public class Engine
    {
        internal readonly string connectionString; //Подключение к БД

        public Engine(string conString)
        {
            this.connectionString = conString;
        }

        public async void CreateTaskAsync(TaskDB model) //Изменение имени в консоли
        {
            using var connection = new NpgsqlConnection(connectionString); //Подключение к БД
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText =
            @"
                INSERT INTO public.tasks (
                    name
                )
                VALUES (
                    @name1
                );
            ";                                                          //Команда на запись и что записать
            command.Parameters.AddWithValue("@name1", model.name); //Запись 
            await command.ExecuteNonQueryAsync(); //Выполнение команды
        }

        public async Task<TaskDB?> ReadTaskAsync(int id)
        {
            using var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText =
            @"
                SELECT id, name
                FROM tasks
                WHERE id = @id;
            ";                                          //Поиск id по имени
            object objectid = id;
            
            command.Parameters.AddWithValue("@id", objectid);
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                TaskDB row = new TaskDB();
                row.id = reader.GetInt32(0);
                row.name = reader.GetString(1);
                return row;
            }
            else
            {
                return null;
            }
        }
        public async Task<List<TaskDB>> ReadAllTasksAsync()
        {
            using var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText =
            @"
                SELECT *
                FROM tasks;
            ";                                                      //Чтение всех полей

            using var reader =await command.ExecuteReaderAsync();

            List <TaskDB> models = new List<TaskDB>(); 
            while (await reader.ReadAsync())
            {
                TaskDB row = new TaskDB();
                row.id = reader.GetInt32(0);
                row.name = reader.GetString(1);
                models.Add(row);            //Запись в лист
            }
            return models; 
        }
        public async Task UpdateAsync(TaskDB taskDB)
        {
            using var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText =
            @"
                UPDATE tasks
                SET name = @name
                WHERE id = @id;
            ";                                             //Перезапись поля в списке
            command.Parameters.AddWithValue("@id", taskDB.id);
            command.Parameters.AddWithValue("@name", taskDB.name);
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText =
            @"
                DELETE FROM tasks
                WHERE id = @id;
            ";                                          //Удаление по id
            command.Parameters.AddWithValue("@id", id);
            await command.ExecuteNonQueryAsync();
        }

        public async Task CreateUser(UserDB model)
        {
            using var connection = new NpgsqlConnection(connectionString); //Подключение к БД
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText =
            @"
                INSERT INTO users (
                    login,
                    password
                )
                VALUES (
                    @login1,
                    @password1
                );
            ";                                                          //Команда на запись и что записать
            command.Parameters.AddWithValue("@login1", model.login); //Запись 
            command.Parameters.AddWithValue("@password1", model.password);
            await command.ExecuteNonQueryAsync(); //Выполнение команды
        }
    }
}