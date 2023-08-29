namespace BusinessObjects.RequestModel
{
    public class CreateTeamReportRequest
    {
        public Guid TeamId { get; set; }
        public string Title { get; set; } = null!;
        public string OverviewReport { get; set; } = null!;
        public string DoneReport { get; set; } = null!;
        public string DoingReport { get; set; } = null!;
        public string? TodoReport { get; set; }
        public int Period { get; set; }
    }
}

