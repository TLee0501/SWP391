namespace BusinessObjects.ResponseModel
{
    public class TeamMemberResponse
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } = null!;
        public string? Mssv { get; set; }
        public bool IsLeader { get; set; }
    }
}
