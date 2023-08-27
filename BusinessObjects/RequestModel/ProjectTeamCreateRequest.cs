namespace BusinessObjects.RequestModel
{
    public class ProjectTeamCreateRequest
    {
        public Guid ProjectId { get; set; }
        public List<Guid>? Users { get; set; }
    }
}
