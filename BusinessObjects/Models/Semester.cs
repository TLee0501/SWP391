﻿using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Semester
{
    public Guid SemesterId { get; set; }

    public string SemeterName { get; set; } = null!;

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public Guid SemesterTypeId { get; set; }
}
