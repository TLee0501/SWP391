using BusinessObjects.Enums;
using BusinessObjects.Models;

namespace BusinessObjects.ResponseModel
{
    public class TaskMemberResponse
    {
        public Guid MemberId { get; set; }
        public string MemberFullName { get; set; } = null!;
    }

    public class TaskResponse
    {
        public Guid TaskId { get; set; }
        public Guid UserId { get; set; }
        public string UserFullName { get; set; } = null!;
        public Guid ProjectId { get; set; }
        public string TaskName { get; set; } = null!;
        public string TaskDescription { get; set; } = null!;
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public ProjectTaskStatus Status { get; set; }
        public List<TaskMemberResponse> Members { get; set; } = null!;
    }
}
