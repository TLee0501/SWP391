using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Project
{
    public Guid ProjectId { get; set; }

    public Guid ClassId { get; set; }

    public string ProjectName { get; set; } = null!;

    public string? Description { get; set; }

    public string? FunctionalReq { get; set; }

    public string? NonfunctionalReq { get; set; }

    public bool IsSelected { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Class Class { get; set; } = null!;

    public virtual ICollection<ProjectTeam> ProjectTeams { get; set; } = new List<ProjectTeam>();

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
