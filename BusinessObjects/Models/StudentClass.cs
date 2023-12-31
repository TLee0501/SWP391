﻿using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class StudentClass
{
    public Guid StudentClassId { get; set; }

    public Guid UserId { get; set; }

    public Guid ClassId { get; set; }

    public virtual Class Class { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
