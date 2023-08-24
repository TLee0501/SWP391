namespace BusinessObjects.ResponseModel
{
    public class UserBasicResponse
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } = null!;
        public string? Mssv { get; set; }
    }
}
