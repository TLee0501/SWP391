namespace BusinessObjects.RequestModel
{
    public class CreateTaskRequest
    {
        public Guid ProjectId { get; set; }
        public string TaskName { get; set; } = null!;
        public string? TaskDescription { get; set; } = null!;
    }
}
