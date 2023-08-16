namespace BusinessObjects.RequestModel
{
    public class StudentCreateTeamRequest
    {
        public Guid ClassID { get; set; }
        public List<Guid> ListStudent { get; set; }
    }
}
