using BusinessObjects.Enums;

namespace BusinessObjects.RequestModel
{
    public class CreateTaskRequest
    {
        public Guid ProjectId { get; set; }
        public string TaskName { get; set; } = null!;
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? TaskDescription { get; set; } = null!;
        public ProjectTaskStatus Status { get; set; }
        public List<string> Assignees { get; set; } = null!;
    }
}
