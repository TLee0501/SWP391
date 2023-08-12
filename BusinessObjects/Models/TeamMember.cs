using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class TeamMember
{
    public Guid TeamMemberId { get; set; }

    public Guid ProjectTeamId { get; set; }

    public Guid UserId { get; set; }

    public virtual ProjectTeam ProjectTeam { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
