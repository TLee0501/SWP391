using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Task
{
    public Guid TaskId { get; set; }

    public Guid UserId { get; set; }

    public Guid ProjectId { get; set; }

    public string TaskName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int Status { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Project Project { get; set; } = null!;

    public virtual ICollection<StudentTask> StudentTasks { get; set; } = new List<StudentTask>();
}
