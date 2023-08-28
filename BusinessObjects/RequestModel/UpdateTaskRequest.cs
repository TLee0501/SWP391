using BusinessObjects.Enums;

namespace BusinessObjects.RequestModel
{
    public class UpdateTaskRequest
    {
        public Guid TaskId { get; set; }
        public string TaskName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public ProjectTaskStatus Status { get; set; }
        public List<Guid> Assignees { get; set; } = null!;
    }

    public class UpdateTaskStatusRequest
    {
        public ProjectTaskStatus Status { get; set; }
    }
}
