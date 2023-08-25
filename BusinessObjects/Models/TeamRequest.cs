﻿using BusinessObjects.Enums;

namespace BusinessObjects.Models;

public partial class TeamRequest
{
    public Guid RequestId { get; set; }

    public Guid UserId { get; set; }

    public Guid ClassId { get; set; }

    public Guid Team { get; set; }

    public Guid ProjectId { get; set; }

    public string TeamName { get; set; } = null!;

    public TeamRequestStatus Status { get; set; }

    public virtual Class Class { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
