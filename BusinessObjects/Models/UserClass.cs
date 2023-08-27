using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class UserClass
{
    public Guid UserClassId { get; set; }

    public Guid ClassId { get; set; }

    public Guid UserId { get; set; }
}
