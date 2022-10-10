using System.Data.Common;
using AutoTestMVC.Models;
using Microsoft.Data.Sqlite;

namespace AutoTestMVC.Repositories
{
    public class QuestionsRepository
    {
        private readonly string _connectionString = "Data source=avtotest.db";
        private readonly SqliteConnection _connection;

        public QuestionsRepository()
        {
            _connection = new SqliteConnection(_connectionString);
        }

        public int GetQuestionsCount()
        {
            _connection.Open();

            var cmd = _connection.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) from questions;";

            var data = cmd.ExecuteReader();

            while (data.Read())
            {
                var count = data.GetInt32(0);

                data.Close();
                _connection.Close();

                return count;
            }

            return 0;
        }

        public QuestionEntity GetQuestionById(int questionId)
        {
            _connection.Open();

            var question = new QuestionEntity();

            var cmd = _connection.CreateCommand();
            cmd.CommandText = "SELECT * from questions WHERE id = @questionId;";
            cmd.Parameters.AddWithValue("@questionId", questionId);
            cmd.Prepare();

            var data = cmd.ExecuteReader();

            while (data.Read())
            {
                question.Id = data.GetInt32(0);
                question.Question = data.GetString(1);
                question.Description = data.GetString(2);
                question.Image = data.GetString(3);
            }

            question.Choices = GetQuestionsChoices(questionId);

            data.Close();
            _connection.Close();

            return question;
        }

        public List<QuestionEntity> GetQuestionsRange(int from = 1, int count = 20)
        {
            _connection.Open();

            var questionEntities = new List<QuestionEntity>();

            for (int i = from; i < from + count; i++)
            {
                questionEntities.Add(GetQuestionById(i));
            }

            _connection.Close();
            return questionEntities;
        }

        public List<Choice> GetQuestionsChoices(int questionId)
        {
            _connection.Open();

            var choices = new List<Choice>();

            var cmd = _connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM choices where questionId = @questionId;";
            cmd.Parameters.AddWithValue("@questionId", questionId);
            cmd.Prepare();

            var data = cmd.ExecuteReader();

            while (data.Read())
            {
                var choice = new Choice()
                {
                    Id = data.GetInt32(0),
                    Text = data.GetString(1),
                    Answer = data.GetBoolean(2)
                };

                choices.Add(choice);
            }

            data.Close();
            _connection.Close();

            return choices;
        }


    }
}
