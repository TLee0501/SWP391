using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Project
{
    public Guid ProjectId { get; set; }

    public Guid CourseId { get; set; }

    public string ProjectName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public bool IsDeleted { get; set; }
}
