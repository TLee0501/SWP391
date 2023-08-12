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

    public virtual Course Course { get; set; } = null!;

    public virtual ICollection<ProjectTeam> ProjectTeams { get; set; } = new List<ProjectTeam>();

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
