using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PPSAP.DTO
{
    public class ExamDTO
    {
        public int ExamId { get; set; }

        [Display(Name = "Exam Title")]

        public string ExamName { get; set; }

        public System.DateTime ExamCreateDate { get; set; }

        public bool IsDeleted { get; set; }

        public int ExamType { get; set; }

        [Display(Name = "Challenge Mode")]
        public bool ExamMode { get; set; }

        public string AdditionalSettingExam { get; set; }

        public int UserId { get; set; }

        [Display(Name = "How Many Questions?")]
        [Required]
        public int NoofQuestions { get; set; }

        [Display(Name = "Timed Exam")]
        public bool ExamTimeType { get; set; }

        [Display(Name = "Show Correct Answers")]
        public bool ExamAnswerToShow { get; set; }

        [Required]
        public List<string> TypeofCategoryList { get; set; }

        [Required]
        public IEnumerable<string> TypeofQuestionList { get; set; }

        [Required]
        public string TypeofCategory { get; set; }

        [Required]
        public string TypeofQuestion { get; set; }

        public int ExamStatus { get; set; }

        public int ExamAttemptId { get; set; }

        public int RoundNumber { get; set; }

        public int SessionId { get; set; }

        public string TypeofCategoryListstring { get; set; }
    }
}
