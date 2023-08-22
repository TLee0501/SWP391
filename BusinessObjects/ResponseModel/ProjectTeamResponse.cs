namespace BusinessObjects.ResponseModel
{
    public class ProjectTeamResponse
    {
        public Guid ProjectTeamId { get; set; }
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string TeamName { get; set; } = null!;
        public DateTime TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public List<UserBasicResponse>? Users { get; set; }
        public string Status { get; set; }
    }
}
