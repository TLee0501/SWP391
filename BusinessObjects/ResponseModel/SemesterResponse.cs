namespace BusinessObjects.ResponseModel
{
    public class SemesterResponse
    {
        public Guid SemesterId { get; set; }
        public string SemesterName { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
