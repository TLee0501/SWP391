namespace BusinessObjects.RequestModel
{
    public class SemesterCreateRequest
    {
        public string SemesterName { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
