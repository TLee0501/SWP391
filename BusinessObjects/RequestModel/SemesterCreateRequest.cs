namespace BusinessObjects.RequestModel
{
    public class SemesterCreateRequest
    {
        public string SemeterName { get; set; } = null!;
        public Guid SemesterTypeId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
