using BusinessObjects.Models;

namespace BusinessObjects.ResponseModel
{
    public class ProjectMemberResponse
    {
        public Guid MemberId { get; set; }
        public string MemberFullName { get; set; } = null!;
        public string MemberCode { get; set; } = null!;
    }

    public class ProjectResponse
    {
        public Guid ProjectId { get; set; }
        public Guid ClassID { get; set; }
        public string ClassName { get; set; } = null!;
        public string ProjectName { get; set; } = null!;
        public string? Description { get; set; }
        public string? FunctionalReq { get; set; }
        public string? NonfunctionalReq { get; set; }
        public bool IsSelected { get; set; }
        public List<ProjectMemberResponse> Members { get; set; } = null!;
    }
}