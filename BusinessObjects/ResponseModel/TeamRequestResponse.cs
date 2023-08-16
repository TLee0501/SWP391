namespace BusinessObjects.ResponseModel
{
    public class TeamRequestResponse
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public Guid TeamId { get; set; }
        public string Status { get; set; } = null!;
    }
}
