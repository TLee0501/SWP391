namespace BusinessObjects.Models
{
    public partial class TeamReport
    {
        public Guid Id { get; set; }
        public Guid TeamId { get; set; }
        public virtual ProjectTeam Team { get; set; } = null!;
        public Guid ReporterId { get; set; }
        public virtual User Reporter { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string OverviewReport { get; set; } = null!;
        public string? DoneReport { get; set; } = null!;
        public string? DoingReport { get; set; } = null!;
        public string? TodoReport { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public int Period { get; set; }
        public virtual TeamReportFeedback? TeacherFeedback { get; set; }
    }
}

