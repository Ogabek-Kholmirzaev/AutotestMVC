namespace AutoTestMVC.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FromIndex { get; set; }
        public int QuestionsCount { get; set; }
        public int CorrectCount { get; set; }
        public bool IsTraining { get; set; }

        public Ticket()
        {

        }

        public  Ticket(int userId, int fromIndex, int questionsCount, int correctCount, bool isTraining)
        {
            UserId = userId;
            FromIndex = fromIndex;
            QuestionsCount = questionsCount;
            CorrectCount = 0;
            IsTraining = isTraining;
        }
    }
}
