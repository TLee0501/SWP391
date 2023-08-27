namespace BusinessObjects.ResponseModel
{
    public class ProjectTeamMember
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string Email { get; set; } = null!;
    }

    public class ProjectTeamInstructor
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
    }

    public class ProjectInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; } = null!;
        public string? FunctionalReq { get; set; } = null!;
        public string? NonfunctionalReq { get; set; } = null!;
    }

    public class ProjectTeamListResponse
    {
        public Guid Id { get; set; }
        public List<ProjectTeamMember> Members { get; set; } = null!;
        public ProjectInfo Project { get; set; } = null!;
        public ProjectTeamInstructor Instructor { get; set; } = null!;
        public ProjectTeamMember Leader { get; set; } = null!;
    }
}

