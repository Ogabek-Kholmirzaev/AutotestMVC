using AutoTestMVC.Models;
using Microsoft.Data.Sqlite;
using System.Data;

namespace AutoTestMVC.Repositories
{
    public class TicketsRepository
    {
        private readonly SqliteConnection _connection;

        public TicketsRepository()
        {
            _connection = new SqliteConnection("Data source=avtotest.db");
            CreateTicket();
        }

        private void CreateTicket()
        {
            _connection.Open();

            var cmd = _connection.CreateCommand();

            cmd.CommandText =
                "CREATE TABLE IF NOT EXISTS tickets(id INTEGER PRIMARY KEY AUTOINCREMENT, user_id INTEGER, from_index INTEGER, questions_count INTEGER, correct_count INTEGER, is_training BOOLEAN)";
            cmd.ExecuteNonQuery();

            cmd.CommandText =
                "CREATE TABLE IF NOT EXISTS tickets_data(id INTEGER PRIMARY KEY AUTOINCREMENT, ticket_id INTEGER, question_id INTEGER, choice_id INTEGER, answer BOOLEAN)";
            cmd.ExecuteNonQuery();

            _connection.Close();
        }

        public void InsertTicket(Ticket ticket)
        {
            _connection.Open();

            var cmd = _connection.CreateCommand();
            cmd.CommandText = "INSERT INTO tickets(user_id, from_index, questions_count, correct_count, is_training) " +
                              $"VALUES({ticket.UserId}, {ticket.FromIndex}, {ticket.QuestionsCount}, {ticket.CorrectCount}, {ticket.IsTraining})";
            cmd.ExecuteNonQuery();

            _connection.Close();
        }

        public Ticket GetTicketById(int id, int userId)
        {
            _connection.Open();

            var ticket = new Ticket();

            var cmd = _connection.CreateCommand();
            cmd.CommandText = $"SELECT * FROM tickets WHERE id = {id} AND user_id = {userId}";

            var data = cmd.ExecuteReader();

            while (data.Read())
            {
                ticket.Id = data.GetInt32(0);
                ticket.UserId = data.GetInt32(1);
                ticket.FromIndex = data.GetInt32(2);
                ticket.QuestionsCount = data.GetInt32(3);
                ticket.CorrectCount = data.GetInt32(4);

            }

            data.Close();
            _connection.Close();

            return ticket;
        }

        public void InsertTicketData(TicketData ticketData)
        {
            _connection.Open();

            var cmd = _connection.CreateCommand();
            cmd.CommandText = "INSERT INTO tickets_data(ticket_id, question_id, choice_id, answer) " +
                              "VALUES(@ticket_id, @question_id, @choice_id, @answer)";
            cmd.Parameters.AddWithValue("@ticket_id", ticketData.TicketId);
            cmd.Parameters.AddWithValue("@question_id", ticketData.QuestionId);
            cmd.Parameters.AddWithValue("@choice_id", ticketData.ChoiceId);
            cmd.Parameters.AddWithValue("@answer", ticketData.Answer);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            _connection.Close();
        }

        public TicketData? GetTicketDataByQuestionId(int ticketId, int questionId)
        {
            _connection.Open();

            var cmd = _connection.CreateCommand();
            cmd.CommandText = $"SELECT * FROM tickets_data WHERE ticket_id={ticketId} AND question_id = {questionId}";

            var data = cmd.ExecuteReader();
            var ticketData = new TicketData();

            while (data.Read())
            {
                ticketData = new TicketData(
                    data.GetInt32(0),
                    data.GetInt32(1),
                    data.GetInt32(2),
                    data.GetInt32(3),
                    data.GetBoolean(4));
            }

            data.Close();
            _connection.Close();

            if (ticketData.QuestionId == questionId)
            {
                return ticketData;
            }

            return null;
        }

        public int GetTicketAnswersCount(int ticketId)
        {
            _connection.Open();

            var cmd = _connection.CreateCommand();
            cmd.CommandText = $"SELECT COUNT(*) FROM tickets_data WHERE ticket_id={ticketId}";
            var data = cmd.ExecuteReader();

            var count = 0;

            while (data.Read())
            {
                count = data.GetInt32(0);
            }
            
            data.Close();
            _connection.Close();

            return count;
        }

        public void UpdateTicketCorrectCount(int ticketId)
        {
            _connection.Open();

            var cmd = _connection.CreateCommand();
            cmd.CommandText = $"UPDATE tickets SET correct_count=correct_count+1 WHERE id={ticketId}";
            cmd.ExecuteNonQuery();

            _connection.Close();
        }

        public List<TicketData> GetTicketDatasById(int ticketId)
        {
            var ticketDatas = new List<TicketData>();

            _connection.Open();

            var cmd = _connection.CreateCommand();
            cmd.CommandText = $"SELECT * FROM tickets_data WHERE ticket_id={ticketId}";

            var data = cmd.ExecuteReader();

            while (data.Read())
            {
                ticketDatas.Add(new TicketData(
                    data.GetInt32(0),
                    data.GetInt32(1),
                    data.GetInt32(2),
                    data.GetInt32(3),
                    data.GetBoolean(4)));
            }

            data.Close();
            _connection.Close();

            return ticketDatas;
        }

        public List<Ticket> GetTicketsByUserId(int userId)
        {
            var tickets = new List<Ticket>();

            _connection.Open();

            var cmd = _connection.CreateCommand();
            cmd.CommandText = $"SELECT id, from_index, questions_count, correct_count FROM tickets WHERE user_id={userId} AND is_training=true";

            var data = cmd.ExecuteReader();

            while (data.Read())
            {
                tickets.Add(new Ticket()
                {
                    Id = data.GetInt32(0),
                    FromIndex = data.GetInt32(1),
                    QuestionsCount = data.GetInt32(2),
                    CorrectCount = data.GetInt32(3)
                });
            }

            data.Close();
            _connection.Close();

            return tickets;
        }

        public int GetLastRowId()
        {
            _connection.Open();

            int id = 0;
            var cmd = _connection.CreateCommand();
            cmd.CommandText = "SELECT id FROM tickets ORDER BY id DESC LIMIT 1";

            var data = cmd.ExecuteReader();

            while (data.Read())
            {
                id = data.GetInt32(0);
            }

            data.Close();
            _connection.Close();

            return id;
        }

        public void InsertUserTrainingTickets(int userId, int ticketsCount, int ticketQuestionsCount)
        {
            for (int i = 0; i < ticketsCount; i++)
            {
                InsertTicket(new Ticket()
                {
                    UserId = userId,
                    CorrectCount = 0,
                    IsTraining = true,
                    FromIndex = i * ticketQuestionsCount + 1,
                    QuestionsCount = ticketQuestionsCount
                });
            }
        }

    }
}
