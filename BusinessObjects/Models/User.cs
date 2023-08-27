using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class User
{
    public Guid UserId { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public Guid RoleId { get; set; }

    public bool IsBan { get; set; }

    public string? Mssv { get; set; }

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<StudentClass> StudentClasses { get; set; } = new List<StudentClass>();

    public virtual ICollection<StudentTask> StudentTasks { get; set; } = new List<StudentTask>();

    public virtual ICollection<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();
}
