using BusinessObjects.Enums;

namespace BusinessObjects.ResponseModel
{
    public class TeamReportDetailResponse
    {
        public Guid Id { get; set; }
        public TeamReporter Reporter { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string OverviewReport { get; set; } = null!;
        public string? DoneReport { get; set; } = null!;
        public string? DoingReport { get; set; } = null!;
        public string? TodoReport { get; set; }
        public DateTime CreatedDate { get; set; }
        public TeamReportDetailResponseFeedback? Feedback { get; set; }
        public int Period { get; set; }
    }

    public class TeamReportDetailResponseFeedback
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = null!;
        public TeamReportFeedbackGrade Grade { get; set; }
        public FeedbackBy FeedbackBy { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
    }

    public class FeedbackBy
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
    }

    public class TeamReporter
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}

