namespace BusinessObjects.Models;

public partial class ProjectTeam
{
    public Guid ProjectTeamId { get; set; }

    public Guid ProjectId { get; set; }

    public string TeamName { get; set; } = null!;

    public int Status { get; set; }

    public Guid LeaderId { get; set; }

    public virtual Project Project { get; set; } = null!;

    public virtual ICollection<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();
}
