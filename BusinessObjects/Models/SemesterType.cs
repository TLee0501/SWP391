using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class SemesterType
{
    public Guid SemesterTypeId { get; set; }

    public string SemesterTypeName { get; set; } = null!;
}
