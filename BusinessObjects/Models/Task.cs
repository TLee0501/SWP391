using BusinessObjects.Enums;

namespace BusinessObjects.Models;

public partial class Task
{
    public Guid TaskId { get; set; }

    public Guid UserId { get; set; }

    public Guid ProjectId { get; set; }

    public string TaskName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public ProjectTaskStatus Status { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Project Project { get; set; } = null!;

    public virtual ICollection<StudentTask> StudentTasks { get; set; } = new List<StudentTask>();
}
