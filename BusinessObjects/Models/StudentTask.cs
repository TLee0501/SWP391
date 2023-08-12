using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class StudentTask
{
    public Guid StudentTaskId { get; set; }

    public Guid TaskId { get; set; }

    public Guid UserId { get; set; }

    public virtual Task Task { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
