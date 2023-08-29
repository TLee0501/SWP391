namespace BusinessObjects.RequestModel
{
    public class UpdateClassDeadlineRequest
    {
        public Guid ClassId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    public class UpdateEnrollCodeRequest
    {
        public Guid ClassId { get; set; }
        public string EnrollCode { get; set; } = null!;
    }
}

