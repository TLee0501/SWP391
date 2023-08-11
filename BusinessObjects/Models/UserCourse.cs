using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class UserCourse
{
    public Guid UserCourseId { get; set; }

    public Guid CourseId { get; set; }

    public Guid UserId { get; set; }
}
