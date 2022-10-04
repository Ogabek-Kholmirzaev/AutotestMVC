using AutoTestMVC.Models;
using Microsoft.Data.Sqlite;

namespace AutoTestMVC.Repositories
{
    public class UsersRepository
    {
        private SqliteConnection? _connection;
        private SqliteCommand? _command;

        public UsersRepository()
        {
            OpenConnection();
            CreateUsersTable();
        }

        private void OpenConnection()
        {
            _connection = new SqliteConnection("Data Source=users.db");
            _connection.Open();
            
            _command = _connection.CreateCommand();
        }
        private void CreateUsersTable()
        {
            _command!.CommandText =
                "CREATE TABLE IF NOT EXISTS users(id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT, phone TEXT, password TEXT)";
            _command.ExecuteNonQuery();
        }

        public void InsertUser(User user)
        {
            _command!.CommandText = $"INSERT INTO users (name, phone, password) VALUES (\"{user.Name}\", \"{user.Phone}\", \"{user.Password}\")";
            _command!.ExecuteNonQuery();
        }

        public User GetUserByPhone(string phone)
        {
            User user = new User();

            _command!.CommandText = $"SELECT * FROM users WHERE phone = \"{phone}\"";
            var data = _command.ExecuteReader();

            while (data.Read())
            {
                user.Index = data.GetInt32(0);
                user.Name = data.GetString(1);
                user.Phone = data.GetString(2);
                user.Password = data.GetString(3);
            }

            return user;
        }
    }
}