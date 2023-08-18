namespace BusinessObjects.RequestModel
{
    public class StudentCreateTeamRequest
    {
        public Guid ClassId { get; set; }
        public Guid ProjectId { get; set; }
        public string TeamName { get; set; }
        public List<Guid> ListStudent { get; set; }
    }
}
