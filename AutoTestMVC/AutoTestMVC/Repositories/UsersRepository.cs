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
        }
        private void CreateUsersTable()
        {
            _connection!.Open();

            _command = _connection.CreateCommand();
            _command!.CommandText =
                "CREATE TABLE IF NOT EXISTS users(id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT, phone TEXT, password TEXT, image TEXT)";
            _command.ExecuteNonQuery();
            _connection.Close();
        }

        public void InsertUser(User user)
        {
            _connection!.Open();

            _command = _connection.CreateCommand();
            _command!.CommandText = $"INSERT INTO users (name, phone, password, image) VALUES (@name, @phone, @password, @image)";
            _command.Parameters.AddWithValue("@name", user.Name);
            _command.Parameters.AddWithValue("@phone", user.Phone);
            _command.Parameters.AddWithValue("@password", user.Password);
            _command.Parameters.AddWithValue("@image", user.Image);
            _command.Prepare();
            _command!.ExecuteNonQuery();
            
            _connection.Close();
        }

        public User GetUserByPhone(string phone)
        {
            _connection!.Open();

            _command = _connection.CreateCommand();

            User user = new User();

            _command!.CommandText = $"SELECT * FROM users WHERE phone = \"{phone}\"";
            var data = _command.ExecuteReader();

            while (data.Read())
            {
                user.Index = data.GetInt32(0);
                user.Name = data.GetString(1);
                user.Phone = data.GetString(2);
                user.Password = data.GetString(3);
                user.Image = data.GetString(4);
            }

            data.Close();
            _connection.Close();

            return user;
        }

        public void UpdateUser(User user)
        {
            _connection!.Open();

            _command = _connection.CreateCommand();

            _command!.CommandText = "UPDATE users SET name=@name, phone=@phone, password=@password, image=@image WHERE id=@userId";
            _command.Parameters.AddWithValue("@name", user.Name);
            _command.Parameters.AddWithValue("@phone", user.Phone);
            _command.Parameters.AddWithValue("@password", user.Password);
            _command.Parameters.AddWithValue("@image", user.Image);
            _command.Parameters.AddWithValue("@userId", user.Index);
            _command.Prepare();

            _command.ExecuteNonQuery();

            _connection.Close();
        }
    }
}