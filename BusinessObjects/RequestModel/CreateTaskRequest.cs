namespace BusinessObjects.RequestModel
{
    public class CreateTaskRequest
    {
        public Guid ProjectId { get; set; }
        public string TaskName { get; set; } = null!;
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? TaskDescription { get; set; } = null!;
    }
}
