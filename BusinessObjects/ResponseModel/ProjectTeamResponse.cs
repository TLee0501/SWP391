namespace BusinessObjects.ResponseModel
{
    public class ProjectTeamResponse
    {
        public Guid ProjectTeamId { get; set; }
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string TeamName { get; set; } = null!;
        public List<TeamMemberResponse>? Users { get; set; }
        public string Status { get; set; }
    }
}
