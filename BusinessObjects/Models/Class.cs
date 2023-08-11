using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Class
{
    public Guid ClassId { get; set; }

    public Guid UserId { get; set; }

    public Guid CourseId { get; set; }

    public string ClassName { get; set; } = null!;

    public DateTime TimeStart { get; set; }

    public DateTime TimeEnd { get; set; }

    public bool IsDeleted { get; set; }

    public bool IsCompleted { get; set; }
}
