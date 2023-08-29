using BusinessObjects.Enums;

namespace BusinessObjects.Models
{
    public partial class TeamReportFeedback
    {
        public Guid Id { get; set; }
        public string? Content { get; set; }
        public Guid TeamReportId { get; set; }
        public virtual TeamReport TeamReport { get; set; } = null!;
        public TeamReportFeedbackGrade Grade { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

