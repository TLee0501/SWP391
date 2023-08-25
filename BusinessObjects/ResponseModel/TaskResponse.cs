using BusinessObjects.Enums;

namespace BusinessObjects.ResponseModel
{
    public class TaskResponse
    {
        public Guid TaskId { get; set; }
        public Guid UserId { get; set; }
        public string UserFullName { get; set; }
        public Guid ProjectId { get; set; }
        public string TaskName { get; set; } = null!;
        public string TaskDescription { get; set; } = null!;
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public ProjectTaskStatus Status { get; set; }
    }
}
