namespace BusinessObjects.ResponseModel
{
    public class TeamRequestResponse
    {
        public Guid TeamId { get; set; }
        public Guid? ProjectId { get; set; }
        public string? ProjectName { get; set; }
        public List<UserBasicResponse> Users { get; set; }
    }
}
