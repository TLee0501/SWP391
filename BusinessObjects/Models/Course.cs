using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Course
{
    public Guid CourseId { get; set; }

    public Guid UserId { get; set; }

    public string CourseCode { get; set; } = null!;

    public string CourseName { get; set; } = null!;

    public DateTime TimeCreated { get; set; }

    public bool IsActive { get; set; }

    public bool IsDelete { get; set; }
}
