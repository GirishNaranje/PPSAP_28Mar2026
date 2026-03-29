namespace PPSAP.DTO
{
    public class ExamHistoryDTO
    {
        public int ExamId { get; set; }

        public int UserId { get; set; }

        public string ExamName { get; set; }

        public int QuestionAttempt { get; set; }

        public int NoofQuestions { get; set; }

        public string ExamType { get; set; }

        public bool ExamTimeType { get; set; }

        public int ExamMode { get; set; }

        // public int Progress { get; set; }
        public int ExamStatus { get; set; }

        public int Score { get; set; }

        public string ExamCreatedDate { get; set; }

        public string ExamLastAttemptDate { get; set; }

        public int LastQuestionId { get; set; }
    }
}
