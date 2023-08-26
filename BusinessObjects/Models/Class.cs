﻿using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Class
{
    public Guid ClassId { get; set; }

    public Guid UserId { get; set; }

    public Guid CourseId { get; set; }

    public string ClassName { get; set; } = null!;

    public string EnrollCode { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public int Status { get; set; }

    public Guid SemesterId { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    public virtual ICollection<StudentClass> StudentClasses { get; set; } = new List<StudentClass>();

    public virtual User User { get; set; } = null!;
}
