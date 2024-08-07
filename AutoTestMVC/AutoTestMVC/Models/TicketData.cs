﻿namespace AutoTestMVC.Models
{
    public class TicketData
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public int QuestionId { get; set; }
        public int ChoiceId { get; set; }
        public bool Answer { get; set; }

        public TicketData()
        {

        }

        public TicketData(int id, int ticketId, int questionId, int choiceId, bool answer)
        {
            Id = id;
            TicketId = ticketId;
            QuestionId = questionId;
            ChoiceId = choiceId;
            Answer = answer;
        }

        public TicketData(int ticketId, int questionId, int choiceId, bool answer)
        {
            TicketId = ticketId;
            QuestionId = questionId;
            ChoiceId = choiceId;
            Answer = answer;
        }
    }
}
