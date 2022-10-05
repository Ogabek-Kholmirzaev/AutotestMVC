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
            var question = new QuestionEntity();

            var cmd = connection.CreateCommand();
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

            return question;
        }

        public List<QuestionEntity> GetQuestionsRange(int from = 1, int count = 20)
        {
            var questionEntities = new List<QuestionEntity>();

            for (int i = from; i < from + count; i++)
            {
                questionEntities.Add(GetQuestionById(i));
            }

            return questionEntities;
        }

        public List<Choice> GetQuestionsChoices(int questionId)
        {
            var choices = new List<Choice>();

            var cmd = connection.CreateCommand();
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

            return choices;
        }


    }
}
