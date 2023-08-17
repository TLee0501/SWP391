namespace BusinessObjects.ResponseModel
{
    public class TeamRequestResponse
    {
        public Guid TeamId { get; set; }
        public List<UserBasicResponse> Users { get; set; }
    }
}
