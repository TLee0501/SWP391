namespace BusinessObjects.ResponseModel
{
    public class SemesterResponse
    {
        public Guid SemesterId { get; set; }
        public string SemeterName { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
