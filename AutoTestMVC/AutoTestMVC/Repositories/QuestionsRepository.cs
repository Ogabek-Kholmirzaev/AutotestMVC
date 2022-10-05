using System.Data.Common;
using AutoTestMVC.Models;
using Microsoft.Data.Sqlite;

namespace AutoTestMVC.Repositories
{
    public class QuestionsRepository
    {
        private readonly string _connectionString = "Data source=avtotest.db";
        private readonly SqliteConnection connection;

        public QuestionsRepository()
        {
            connection = new SqliteConnection(_connectionString);
            connection.Open();
        }

        public int GetQuestionsCount()
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) from questions;";

            var data = cmd.ExecuteReader();

            while (data.Read())
            {
                return data.GetInt32(0);
            }

            return 0;
        }

        public QuestionEntity GetQuestionById(int questionId)
        {
            return null;
        }

        public List<QuestionEntity> GetQuestionsRange(int from, int count)
        {
            return null;
        }

        public List<Choice> GetQQuestionsChoices()
        {
            return null;
        }


    }
}
